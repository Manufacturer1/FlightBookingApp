using Microsoft.AspNetCore.Http;
using ServerLibrary.Extensions;

namespace ServerLibrary.Memento
{
    public class BookingWizardHistory
    {
        private const string UndoStackKey = "BookingWizard_UndoStack";
        private const string RedoStackKey = "BookingWizard_RedoStack";

        private readonly ISession _session;

        public BookingWizardHistory(ISession session)
        {
            _session = session ?? throw new ArgumentNullException(nameof(session));
        }
        public void SaveState(BookingDraft draft)
        {
            if (draft == null) throw new ArgumentNullException(nameof(draft));

            var undoStack = GetUndoStack();
            var redoStack = GetRedoStack();

            undoStack.Push(draft.Save());
            redoStack.Clear();

            SaveStacks(undoStack, redoStack);
        }

        public BookingDraftMemento? Undo()
        {
            var undoStack = GetUndoStack();
            var redoStack = GetRedoStack();

            if (undoStack.Count == 0)
                throw new InvalidOperationException("Nothing to undo");

            var currentState = undoStack.Pop();
            redoStack.Push(currentState);

            SaveStacks(undoStack, redoStack);

            return undoStack.Count > 0 ? undoStack.Peek() : null;
        }

        public BookingDraftMemento Redo()
        {
            var undoStack = GetUndoStack();
            var redoStack = GetRedoStack();

            if (redoStack.Count == 0)
                throw new InvalidOperationException("Nothing to redo");

            var nextState = redoStack.Pop();
            undoStack.Push(nextState);

            SaveStacks(undoStack, redoStack);

            return nextState;
        }
        
        public bool CanUndo() => GetUndoStack().Count > 0;
        public bool CanRedo() => GetRedoStack().Count > 0;
        public Stack<BookingDraftMemento> GetUndoStack()
        {
            var list = _session.Get<List<BookingDraftMemento>>(UndoStackKey);
            return list == null ? new Stack<BookingDraftMemento>() : new Stack<BookingDraftMemento>(list.Reverse<BookingDraftMemento>());
        }
        private Stack<BookingDraftMemento> GetRedoStack()
        {
            var list = _session.Get<List<BookingDraftMemento>>(RedoStackKey);
            return list == null ? new Stack<BookingDraftMemento>() : new Stack<BookingDraftMemento>(list.Reverse<BookingDraftMemento>());
        }

        private void SaveStacks(Stack<BookingDraftMemento> undoStack, Stack<BookingDraftMemento> redoStack)
        {

            _session.Set(UndoStackKey, undoStack.ToList());
            _session.Set(RedoStackKey, redoStack.ToList());
        }
        public void ClearHistory()
        {
            _session.Remove(UndoStackKey);
            _session.Remove(RedoStackKey);
        }
    }
}
