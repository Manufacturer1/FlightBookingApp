using ServerLibrary.Command;
using ServerLibrary.Services.Interfaces;

namespace ServerLibrary.FactoryMethod
{
    public class GenerateTicketCommandCreator : ICreateCommand<GenerateTicketsCommand>
    {
        private readonly ITicketService _ticketService;

        public GenerateTicketCommandCreator(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        public IBookingCommand CreateCommand(params object[] args)
        {
            var createBookingCmd = (CreateBookingCommand)args[0];
            return new GenerateTicketsCommand(_ticketService, createBookingCmd);
        }
    }
}
