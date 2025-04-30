using AutoMapper;
using BaseEntity.Dtos;
using BaseEntity.Entities;
using BaseEntity.Responses;
using Microsoft.AspNetCore.Http;
using ServerLibrary.Command;
using ServerLibrary.Memento;
using ServerLibrary.Repositories.Interfaces;
using ServerLibrary.Services.Interfaces;
using ServerLibrary.Validators;
using System.Transactions;

namespace ServerLibrary.Services.Implementations
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IFlightRepository _flightRepository;
        private readonly IItineraryRepository _itineraryRepository;
        private IMapper _mapper;
        private readonly BookingWizardHistory _bookingHistory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IPassengerRepository _passengerRepository;
        private readonly IPassportIdentityRepository _passportIdentityRepository;
        private readonly IContactRepository _contactRepository;
        private readonly ITicketService _ticketService;
        private readonly IContactValidator _contactValidator;
        private readonly IPassportValidator _passportValidator;
        private readonly IPassengerValidator _passengerValidator;

        public BookingService(IBookingRepository bookingRepository,
            IFlightRepository flightRepository,
            IItineraryRepository itineraryRepository,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
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
            _mapper = mapper;
            _itineraryRepository = itineraryRepository;
            _httpContextAccessor = httpContextAccessor;
            _bookingHistory = new BookingWizardHistory(httpContextAccessor.HttpContext!.Session);
            _passengerRepository = passengerRepository;
            _passportIdentityRepository = passportIdentityRepository;
            _contactRepository = contactRepository;
            _ticketService = ticketService;
            _contactValidator = contactValidator;
            _passportValidator = passportValidator;
            _passengerValidator = passengerValidator;
        }

        public async Task<BookingResponse> BookSeatAsync(CreateBookingDto createBooking)
        {
            var draft = GetCurrentBookingState();
            if (draft == null)
                return new BookingResponse(false, "Draft is null", null);

            var itineraryCmd = new ValidateItineraryCommand(_itineraryRepository, createBooking.ItineraryId);
            var validateDraftCmd = new ValidateBookingDraftCommand(draft);
            var validateSectionDraftCommand = new ValidateBookingDraftDetailsCommand(_contactValidator, _passengerValidator, _passportValidator, draft);

            var processPassengerCmd = new ProcessPassengerCommand(_mapper, _passengerRepository, _contactRepository,_passportIdentityRepository, draft);

            var updateSeatCmd = new UpdateSeatAvailabilityCommand(_flightRepository, itineraryCmd,createBooking.PassengerNumberSelected);

            var createBookingCmd = new CreateBookingCommand(_mapper, _bookingRepository,createBooking, processPassengerCmd, draft);

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

            return new BookingResponse(result.Success,result.Message,generateTicketsCmd.GeneratedTickets);
         
        }

    public async Task<IEnumerable<GetBookingDto>> GetAllBookingsAsync()
        {
            var bookings = await _bookingRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<GetBookingDto>>(bookings);
        }

        public async Task<GetBookingDto?> GetBookingAsync(int bookingId)
        {
            var booking = await _bookingRepository.GetAsync(bookingId);
            return _mapper.Map<GetBookingDto>(booking);
        }

        public async Task<GeneralReponse> RemoveBookingAsync(int bookingId)
        {
            return await _bookingRepository.RemoveAsync(bookingId);
        }
        //Memento
        public void SaveBookingState(BookingDraft booking)
        {
            if (booking == null) throw new ArgumentNullException(nameof(booking));

            _bookingHistory.SaveState(booking);
        }
        public BookingDraftMemento? GetCurrentBookingState()
        {
            if (!_bookingHistory.CanUndo()) return null;

            var currentState = _bookingHistory.GetUndoStack().Peek();
            return currentState;
        }
        public void ClearBookingHistory()
        {
            _httpContextAccessor.HttpContext!.Session.Clear();
            _httpContextAccessor.HttpContext!.Response.Cookies.Delete(".AspNetCore.Session", new CookieOptions
            {
                Path = "/",
                Secure = true,
                HttpOnly = true,
                SameSite = SameSiteMode.None
            });
        }


    }
}
