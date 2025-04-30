using ServerLibrary.Command.CommandResults;
using ServerLibrary.Memento;

namespace ServerLibrary.Command
{
    public class ValidateBookingDraftCommand : IBookingCommand
    {
        private BookingDraftMemento _draft;

        public ValidateBookingDraftCommand(BookingDraftMemento draft)
        {
            _draft = draft;
            
        }
        public Task<CommandResult> ExecuteAsync()
        {
            if (_draft == null)
                return Task.FromResult(CommandResult.Failure("Booking history missing"));

            if (_draft.ContactDetails == null || _draft.Passenger == null || _draft.Passport == null)
                return Task.FromResult(CommandResult.Failure("Incomplete booking draft"));

            if (string.IsNullOrEmpty(_draft.PaymentIntentId))
                return Task.FromResult(CommandResult.Failure("Payment intent is null."));

            

            return Task.FromResult(CommandResult.Success(_draft));
        }
    }
}
