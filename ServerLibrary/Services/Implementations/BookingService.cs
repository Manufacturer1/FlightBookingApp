using AutoMapper;
using BaseEntity.Dtos;
using BaseEntity.Responses;
using Microsoft.AspNetCore.Http;
using ServerLibrary.Facade;
using ServerLibrary.Memento;
using ServerLibrary.Repositories.Interfaces;
using ServerLibrary.Services.Interfaces;

namespace ServerLibrary.Services.Implementations
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private IMapper _mapper;
        private readonly BookingWizardHistory _bookingHistory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IBookingFacade _bookingFacade;


        public BookingService(IBookingRepository bookingRepository,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            IBookingFacade bookingFacade)

        {
            _bookingRepository = bookingRepository;
            _httpContextAccessor = httpContextAccessor;
            _bookingFacade = bookingFacade;
            _mapper = mapper;
            _bookingHistory = new BookingWizardHistory(httpContextAccessor.HttpContext!.Session);

        }

        public async Task<BookingResponse> BookSeatAsync(CreateBookingDto createBooking)
        {

            var draft = GetCurrentBookingState();
            return await _bookingFacade.BookSeatAsync(createBooking,draft!);
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
