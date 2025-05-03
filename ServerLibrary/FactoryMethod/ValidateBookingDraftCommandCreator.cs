using ServerLibrary.Command;
using ServerLibrary.Memento;

namespace ServerLibrary.FactoryMethod
{
    public class ValidateBookingDraftCommandCreator : ICreateCommand<ValidateBookingDraftCommand>
    {
        public IBookingCommand CreateCommand(params object[] args)
        {
            BookingDraftMemento draft = (BookingDraftMemento)args[0];
            return new ValidateBookingDraftCommand(draft);
        }
    }
}
