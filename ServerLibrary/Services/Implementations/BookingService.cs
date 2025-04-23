using AutoMapper;
using BaseEntity.Dtos;
using BaseEntity.Entities;
using BaseEntity.Responses;
using ServerLibrary.Repositories.Interfaces;
using ServerLibrary.Services.Interfaces;

namespace ServerLibrary.Services.Implementations
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IFlightRepository _flightRepository;
        private readonly IItineraryRepository _itineraryRepository;
        private IMapper _mapper;

        public BookingService(IBookingRepository bookingRepository,
            IFlightRepository flightRepository,
            IItineraryRepository itineraryRepository,
            IMapper mapper)
        {
            _bookingRepository = bookingRepository;
            _flightRepository = flightRepository;
            _mapper = mapper;
            _itineraryRepository = itineraryRepository;
        }
        public async Task<GeneralReponse> BookSeatAsync(CreateBookingDto createBooking)
        {
            var itinerary = await _itineraryRepository.GetByIdAsync(createBooking.ItineraryId);
            if (itinerary == null) return new GeneralReponse(false, "Itinerary does not exist.");
          
            foreach(var flight in itinerary.Segments!)
            {
                var existingFlight = await _flightRepository.GetByFlightNumberAsync(flight.FlightNumber);
                if (existingFlight == null)
                    return new GeneralReponse(false, "Flight does not exist.");

                if(existingFlight.AvailableSeats > 0)
                {
                    existingFlight.AvailableSeats -= createBooking.PassengerNumberSelected;
                    await _flightRepository.UpdateAsync(existingFlight);
                }
            }
            var booking = _mapper.Map<Booking>(createBooking);

            return await _bookingRepository.CreateAsync(booking);
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
    }
}
