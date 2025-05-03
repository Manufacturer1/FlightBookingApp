using AutoMapper;
using BaseEntity.Dtos;
using ServerLibrary.Command;
using ServerLibrary.Memento;
using ServerLibrary.Repositories.Interfaces;

namespace ServerLibrary.FactoryMethod
{ 
    public class CreateBookingCommandCreator : ICreateCommand<CreateBookingCommand>
    {
        private readonly IMapper _mapper;
        private readonly IBookingRepository _bookingRepo;
        public CreateBookingCommandCreator(IMapper mapper, IBookingRepository bookingRepo)
        {
            _mapper = mapper;
            _bookingRepo = bookingRepo;
        }

        public IBookingCommand CreateCommand(params object[] args)
        {
            var createBookingDto = (CreateBookingDto)args[0];
            var processPassengerCmd = (ProcessPassengerCommand)args[1];
            var draft = (BookingDraftMemento)args[2];
            return new CreateBookingCommand(_mapper, _bookingRepo, createBookingDto, processPassengerCmd, draft);
        }
    }
}
