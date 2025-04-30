using AutoMapper;
using BaseEntity.Dtos;
using BaseEntity.Responses;
using Microsoft.AspNetCore.Http;
using ServerLibrary.Command;
using ServerLibrary.Memento;
using ServerLibrary.Repositories.Interfaces;
using ServerLibrary.Services.Interfaces;
using ServerLibrary.Validators;

namespace ServerLibrary.Facade
{
    public class BookingFacade : IBookingFacade
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IFlightRepository _flightRepository;
        private readonly IItineraryRepository _itineraryRepository;
        private IMapper _mapper;
        private readonly IPassengerRepository _passengerRepository;
        private readonly IPassportIdentityRepository _passportIdentityRepository;
        private readonly IContactRepository _contactRepository;
        private readonly ITicketService _ticketService;
        private readonly IContactValidator _contactValidator;
        private readonly IPassportValidator _passportValidator;
        private readonly IPassengerValidator _passengerValidator;


        public BookingFacade(
            IBookingRepository bookingRepository,
            IFlightRepository flightRepository,
            IItineraryRepository itineraryRepository,
            IMapper mapper,
            IPassengerRepository passengerRepository,
            IPassportIdentityRepository passportIdentityRepository,
            IContactRepository contactRepository,
            ITicketService ticketService,
            IContactValidator contactValidator,
            IPassportValidator passportValidator,
            IPassengerValidator passengerValidator)
        {
            _bookingRepository = bookingRepository;
            _flightRepository = flightRepository;
            _itineraryRepository = itineraryRepository;
            _mapper = mapper;
            _passengerRepository = passengerRepository;
            _passportIdentityRepository = passportIdentityRepository;
            _contactRepository = contactRepository;
            _ticketService = ticketService;
            _contactValidator = contactValidator;
            _passportValidator = passportValidator;
            _passengerValidator = passengerValidator;

        }

        public async Task<BookingResponse> BookSeatAsync(CreateBookingDto createBooking,BookingDraftMemento draft)
        {
            if (draft == null)
                return new BookingResponse(false, "Draft is null", null);

            var itineraryCmd = new ValidateItineraryCommand(_itineraryRepository, createBooking.ItineraryId);
            var validateDraftCmd = new ValidateBookingDraftCommand(draft);
            var validateSectionDraftCommand = new ValidateBookingDraftDetailsCommand(_contactValidator, _passengerValidator, _passportValidator, draft);

            var processPassengerCmd = new ProcessPassengerCommand(_mapper, _passengerRepository, _contactRepository, _passportIdentityRepository, draft);

            var updateSeatCmd = new UpdateSeatAvailabilityCommand(_flightRepository, itineraryCmd, createBooking.PassengerNumberSelected);

            var createBookingCmd = new CreateBookingCommand(_mapper, _bookingRepository, createBooking, processPassengerCmd, draft);

            var generateTicketsCmd = new GenerateTicketsCommand(_ticketService, createBookingCmd);


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
