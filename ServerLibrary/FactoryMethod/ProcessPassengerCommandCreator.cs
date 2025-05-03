using AutoMapper;
using ServerLibrary.Command;
using ServerLibrary.Memento;
using ServerLibrary.Repositories.Interfaces;

namespace ServerLibrary.FactoryMethod
{
    public class ProcessPassengerCommandCreator : ICreateCommand<ProcessPassengerCommand>
    {
        private readonly IMapper _mapper;
        private readonly IPassengerRepository _passengerRepo;
        private readonly IContactRepository _contactRepo;
        private readonly IPassportIdentityRepository _passportRepo;

        public ProcessPassengerCommandCreator(IMapper mapper, IPassengerRepository passengerRepo, IContactRepository contactRepo, IPassportIdentityRepository passportRepo)
        {
            _mapper = mapper;
            _passengerRepo = passengerRepo;
            _contactRepo = contactRepo;
            _passportRepo = passportRepo;
        }

        public IBookingCommand CreateCommand(params object[] args)
        {
            BookingDraftMemento draft = (BookingDraftMemento)args[0];
            return new ProcessPassengerCommand(_mapper, _passengerRepo, _contactRepo, _passportRepo, draft);
        }
    }
}
