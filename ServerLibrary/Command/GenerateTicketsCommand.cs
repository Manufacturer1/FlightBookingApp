using BaseEntity.Dtos;
using BaseEntity.Entities;
using ServerLibrary.Command.CommandResults;
using ServerLibrary.Services.Interfaces;

namespace ServerLibrary.Command
{
    public class GenerateTicketsCommand : IBookingCommand
    {
        private readonly ITicketService _ticketService;
        private readonly CreateBookingCommand _bookingCommand;

        public List<GetTicketDto>? GeneratedTickets { get; private set; }

        public GenerateTicketsCommand(
             ITicketService ticketService,
             CreateBookingCommand bookingCommand)
        {
            _ticketService = ticketService;
            _bookingCommand = bookingCommand;
        }
        public async Task<CommandResult> ExecuteAsync()
        {
            var bookingId = _bookingCommand.BookingId;
            if (!bookingId.HasValue)
                return CommandResult.Failure("Cannot generate tickets without booking.");

            var (result, tickets) = await _ticketService.GenerateTicketAsync(new CreateTicketDto { BookingId = bookingId.Value });

            if (!result.Flag || tickets == null || !tickets.Any())
                return CommandResult.Failure(result.Message);

            GeneratedTickets = tickets;

            return CommandResult.Success();
        }
    }
}
