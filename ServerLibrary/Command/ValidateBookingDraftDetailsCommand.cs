using ServerLibrary.Command.CommandResults;
using ServerLibrary.Memento;
using ServerLibrary.Validators;

namespace ServerLibrary.Command
{
    public class ValidateBookingDraftDetailsCommand : IBookingCommand
    {
        private readonly IContactValidator _contactValidator;
        private readonly IPassengerValidator _passengerValidator;
        private readonly IPassportValidator _passportValidator;
        private readonly BookingDraftMemento _draft;

        public ValidateBookingDraftDetailsCommand(
                IContactValidator contactValidator,
                IPassengerValidator passengerValidator,
                IPassportValidator passportValidator,
                BookingDraftMemento draft)
        {
            _contactValidator = contactValidator;
            _passengerValidator = passengerValidator;
            _passportValidator = passportValidator;
            _draft = draft;
        }
        public Task<CommandResult> ExecuteAsync()
        {
            var contactResult = _contactValidator.Validate(_draft.ContactDetails!);
            if (!contactResult.IsValid) return Task.FromResult(CommandResult.Failure(contactResult.ErrorMessage));

            var passengerResult = _passengerValidator.Validate(_draft.Passenger!);
            if (!passengerResult.IsValid) return Task.FromResult(CommandResult.Failure(passengerResult.ErrorMessage));

            var passportResult = _passportValidator.Validate(_draft.Passport!);
            if (!passportResult.IsValid) return Task.FromResult(CommandResult.Failure(passportResult.ErrorMessage));

            return Task.FromResult(CommandResult.Success());
        }
    }
}
