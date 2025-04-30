using AutoMapper;
using BaseEntity.Entities;
using ServerLibrary.Command.CommandResults;
using ServerLibrary.Memento;
using ServerLibrary.Repositories.Interfaces;

namespace ServerLibrary.Command
{
    public class ProcessPassengerCommand : IBookingCommand
    {
        private readonly IPassengerRepository _passengerRepo;
        private readonly IContactRepository _contactRepo;
        private readonly IPassportIdentityRepository _passportRepo;
        private readonly BookingDraftMemento _draft;
        private readonly IMapper _mapper;

        public int PassengerId { get; private set; }

        public ProcessPassengerCommand(
               IMapper mapper,
               IPassengerRepository passengerRepo,
               IContactRepository contactRepo,
               IPassportIdentityRepository passportRepo,
               BookingDraftMemento draft)
        {
            _mapper = mapper;
            _passengerRepo = passengerRepo;
            _contactRepo = contactRepo;
            _passportRepo = passportRepo;
            _draft = draft;
        }

        public async Task<CommandResult> ExecuteAsync()
        {
            var contact = _mapper.Map<ContactDetails>(_draft.ContactDetails);
            var passport = _mapper.Map<PassportIdentity>(_draft.Passport);
            var passenger = _mapper.Map<Passenger>(_draft.Passenger);

            var allPassengers = await _passengerRepo.GetAllAsync();
            var existing = allPassengers
                .FirstOrDefault(p => p.ContactDetails?.Email.Equals(_draft.ContactDetails!.Email, StringComparison.OrdinalIgnoreCase) == true);

            if (existing != null)
            {
                contact.Id = existing.ContactDetailsId;
                passport.Id = existing.PassportIdentityId;
                passenger.Id = existing.Id;

                if (!(await _contactRepo.UpdateAsync(contact)).Flag)
                    return CommandResult.Failure("Failed to update contact.");

                if (!(await _passportRepo.UpdateAsync(passport)).Flag)
                    return CommandResult.Failure("Failed to update passport.");

                if (!(await _passengerRepo.UpdateAsync(passenger)).Flag)
                    return CommandResult.Failure("Failed to update passenger.");

                PassengerId = existing.Id;
            }
            else
            {
                var contactId = await _contactRepo.CreateAsync(contact);
                var passportId = await _passportRepo.CreateAsync(passport);

                passenger.ContactDetailsId = contactId;
                passenger.PassportIdentityId = passportId;

                PassengerId = await _passengerRepo.CreateAsync(passenger);
            }
            return CommandResult.Success();
        }
    }
}
