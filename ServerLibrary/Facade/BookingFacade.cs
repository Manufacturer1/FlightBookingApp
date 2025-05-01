using BaseEntity.Dtos;
using BaseEntity.Responses;
using ServerLibrary.Command;
using ServerLibrary.FactoryMethod;
using ServerLibrary.Memento;

namespace ServerLibrary.Facade
{
    public class BookingFacade : IBookingFacade
    {

        private readonly IBookingCommandFactory _commandFactory;

        public BookingFacade(IBookingCommandFactory commandFactory)
        {
            _commandFactory = commandFactory;
        }

        public async Task<BookingResponse> BookSeatAsync(CreateBookingDto createBooking,BookingDraftMemento draft)
        {
            if (draft == null)
                return new BookingResponse(false, "Draft is null", null);

            var itineraryCmd = _commandFactory.CreateValidateItineraryCommand(createBooking.ItineraryId);

            var validateDraftCmd = _commandFactory.CreateValidateBookingDraftCommand(draft);

            var validateSectionDraftCommand = _commandFactory.CreateBookingDraftDetailsCommand(draft);

            var processPassengerCmd = _commandFactory.CreateProcessPassangerCommand(draft);

            var updateSeatCmd = _commandFactory.CreateUpdateSeatCommand(itineraryCmd, createBooking.PassengerNumberSelected);

            var createBookingCmd = _commandFactory.CreateCreateBookingCommand(createBooking, processPassengerCmd, draft);

            var generateTicketsCmd = _commandFactory.CreateGenerateTicketsCommand(createBookingCmd);


            var handler = new BookingCommandInvoker();
            handler.AddCommand(itineraryCmd);
            handler.AddCommand(validateDraftCmd);
            handler.AddCommand(validateSectionDraftCommand);
            handler.AddCommand(processPassengerCmd);
            handler.AddCommand(updateSeatCmd);
            handler.AddCommand(createBookingCmd);
            handler.AddCommand(generateTicketsCmd);


            var result = await handler.ExecuteAllAsync();

            return new BookingResponse(result.Success, result.Message, generateTicketsCmd.GeneratedTickets);
        }
    }
}
