using AutoMapper;
using BaseEntity.Dtos;
using BaseEntity.Entities;
using ServerLibrary.Command.CommandResults;
using ServerLibrary.Memento;
using ServerLibrary.Repositories.Interfaces;

namespace ServerLibrary.Command
{
    public class CreateBookingCommand : IBookingCommand
    {
        private readonly IMapper _mapper;
        private readonly IBookingRepository _bookingRepo;
        private readonly CreateBookingDto _dto;
        private readonly ProcessPassengerCommand _passengerCommand;
        private readonly BookingDraftMemento _draft;
     
        public int? BookingId { get; private set; }

        public CreateBookingCommand(
            IMapper mapper,
            IBookingRepository bookingRepo,
            CreateBookingDto dto,
            ProcessPassengerCommand passengerCommand,
            BookingDraftMemento draft)
        {
            _mapper = mapper;
            _bookingRepo = bookingRepo;
            _dto = dto;
            _passengerCommand = passengerCommand;
            _draft = draft;
        }
        public async Task<CommandResult> ExecuteAsync()
        {

            var booking = _mapper.Map<Booking>(_dto);
            booking.PassengerId = _passengerCommand.PassengerId;
            booking.PaymentIntentId = _draft.PaymentIntentId!;
            booking.BookingDate = DateTime.Now;

            var (result,id) = await _bookingRepo.CreateAsync(booking);

            if (!result.Flag || !id.HasValue)
                return CommandResult.Failure(result.Message);

            BookingId = id.Value;


            return CommandResult.Success();
        }
    }
}
