using BaseEntity.Dtos;
using BaseEntity.Enums;
using BaseEntity.Responses;
using ServerLibrary.Command;
using ServerLibrary.FactoryMethod;
using ServerLibrary.Memento;

namespace ServerLibrary.Facade
{
    public class BookingFacade : IBookingFacade
    {

        private readonly IBookingCommandCreatorFactory _commandCreatorFactory;

        public BookingFacade(IBookingCommandCreatorFactory commandCreatorFactory)
        {
            _commandCreatorFactory = commandCreatorFactory;
        }

        public async Task<BookingResponse> BookSeatAsync(CreateBookingDto createBooking,BookingDraftMemento draft)
        {
            if (draft == null)
                return new BookingResponse(false, "Draft is null", null);

            var itineraryCmd = _commandCreatorFactory
                  .CreateBookingCommand<ValidateItineraryCommand>(BookingCommandType.ValidateItinerary)
                  .CreateCommand(createBooking.ItineraryId);

            var validateDraftCmd = _commandCreatorFactory
                .CreateBookingCommand<ValidateBookingDraftCommand>(BookingCommandType.ValidateDraft)
                .CreateCommand(draft);

            var validateSectionDraftCommand = _commandCreatorFactory
                .CreateBookingCommand<ValidateBookingDraftDetailsCommand>(BookingCommandType.ValidateSection)
                .CreateCommand(draft);

            var processPassengerCmd = _commandCreatorFactory
                .CreateBookingCommand<ProcessPassengerCommand>(BookingCommandType.ProcessPassenger)
                .CreateCommand(draft);

            var updateSeatCmd = _commandCreatorFactory
                .CreateBookingCommand<UpdateSeatAvailabilityCommand>(BookingCommandType.UpdateSeat)
                .CreateCommand(itineraryCmd!, createBooking.PassengerNumberSelected);

            var createBookingCmd = _commandCreatorFactory
                .CreateBookingCommand<CreateBookingCommand>(BookingCommandType.CreateBooking)
                .CreateCommand(createBooking, processPassengerCmd!, draft);

            var generateTicketsCmd = _commandCreatorFactory
                .CreateBookingCommand<GenerateTicketsCommand>(BookingCommandType.GenerateTickets)
                .CreateCommand(createBookingCmd!) as GenerateTicketsCommand;


            var handler = new BookingCommandInvoker();
            handler.AddCommand(itineraryCmd!);
            handler.AddCommand(validateDraftCmd!);
            handler.AddCommand(validateSectionDraftCommand!);
            handler.AddCommand(processPassengerCmd!);
            handler.AddCommand(updateSeatCmd!);
            handler.AddCommand(createBookingCmd!);
            handler.AddCommand(generateTicketsCmd!);


            var result = await handler.ExecuteAllAsync();

            return new BookingResponse(result.Success, result.Message,generateTicketsCmd!.GeneratedTickets);
        }
    }
}
