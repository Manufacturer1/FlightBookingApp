using ServerLibrary.Command;
using ServerLibrary.Memento;
using ServerLibrary.Validators;

namespace ServerLibrary.FactoryMethod
{
    public class ValidateBookingDraftDetailsCreator : ICreateCommand<ValidateBookingDraftDetailsCommand>
    {
        private readonly IContactValidator _contactValidator;
        private readonly IPassengerValidator _passengerValidator;
        private readonly IPassportValidator _passportValidator;

        public ValidateBookingDraftDetailsCreator(IContactValidator contactValidator, IPassengerValidator passengerValidator, IPassportValidator passportValidator)
        {
            _contactValidator = contactValidator;
            _passengerValidator = passengerValidator;
            _passportValidator = passportValidator;
        }


        public IBookingCommand CreateCommand(params object[] args)
        {
            BookingDraftMemento draft = (BookingDraftMemento)args[0];
            return new ValidateBookingDraftDetailsCommand(_contactValidator, _passengerValidator, _passportValidator, draft);
        }
    }
}
