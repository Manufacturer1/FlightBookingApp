using AutoMapper;
using BaseEntity.Dtos;
using BaseEntity.Entities;
using BaseEntity.Responses;
using ServerLibrary.Repositories.Interfaces;

namespace ServerLibrary.Builder
{
    public class TicketBuilder : ITicketBuilder
    {
        private Booking? _booking;
        private Airline? _airline;
        private FlightSegment? _flightSegment;
        private Flight? _flight;

        private readonly IFlightRepository _flightRepository;
        private readonly IMapper _mapper;
        private readonly IBookingRepository _bookingRepository;
        private readonly IItineraryRepository _tineraryRepository;
        private readonly IAirlineRepository _airlineRepository;
        private readonly ITicketRepository _ticketRepository;


        public TicketBuilder(IFlightRepository flightRepo,
                              IBookingRepository bookingRepo,
                              IItineraryRepository itineraryRepo,
                              IAirlineRepository airlineRepo,
                              ITicketRepository ticketRepository,
                              IMapper mapper)
        {
            _flightRepository = flightRepo;
            _mapper = mapper;
            _bookingRepository = bookingRepo;
            _airlineRepository = airlineRepo;
            _tineraryRepository = itineraryRepo;
            _ticketRepository = ticketRepository;

        }
        public void SetBooking(Booking booking)
            => _booking = booking;

        public void SetAirline(Airline airline)
            => _airline = airline;

        public void SetFlightSegment(FlightSegment flightSegment)
            => _flightSegment = flightSegment;

        public void SetFlight(Flight flight)
            => _flight = flight;

        public Ticket BuildTicket()
        {
            if (_booking == null || _flightSegment == null || _flight == null || _airline == null)
                throw new InvalidOperationException("Missing data for ticket creation.");


            if (!TimeSpan.TryParse(_flightSegment.Flight!.DepartureTime, out var departureTime))
                throw new FormatException("Invalid departure time.");

            var departureDateTime = _flight.DepartureDate.Date + departureTime;
            var checkInDate = departureDateTime.AddMinutes(-10);

            return new Ticket
            {
                BookingId = _booking.Id,
                AirlineBookingCode = GenerateAirlineBookingCode(_airline.IataCode),
                CheckInDate = checkInDate,
                IssueDate = DateTime.Now,
                FlightNumber = _flight.FlightNumber,
                PaymentIntentId = _booking.PaymentIntentId,
            };
        }

        public async Task<GetTicketDto> BuildTicketDtoAsync(int ticketId)
        {
            var ticket = await _ticketRepository.GetAsync(ticketId);
            if(ticket == null) throw new Exception("Ticket not found");

            var flight = await _flightRepository.GetByFlightNumberAsync(ticket.FlightNumber);
            var booking = await _bookingRepository.GetAsync(ticket.BookingId);
            var itinerary = await _tineraryRepository.GetByIdAsync(booking!.ItineraryId);
            var airline = await _airlineRepository.GetByIdAsync(itinerary!.AirlineId);

            if (airline == null) throw new Exception("Airline was not found.");
            if (flight == null) throw new Exception("Flight was not found.");


            if (!TimeSpan.TryParse(flight.DepartureTime, out var depTime) || !TimeSpan.TryParse(flight.ArrivalTime, out var arrTime))
                throw new Exception("Invalid time");

            return new GetTicketDto
            {
                Id = ticket.Id,
                BookingId = booking.Id,
                FlightNumber = ticket.FlightNumber,
                CheckInDate = ticket.CheckInDate,
                IssueDate = ticket.IssueDate,
                AirlineBookingCode = ticket.AirlineBookingCode,
                Airline = _mapper.Map<GetAirlineDto>(airline),
                Baggage = _mapper.Map<GetBaggageDto>(airline.BaggagePolicy),
                PassengerName = booking.Passenger!.Name,
                PassengerSurname = booking.Passenger.Surname,
                Origin = flight.Origin,
                Destination = flight.Destination,
                ClassType = flight.ClassType!,
                DepartureTime = flight.DepartureTime,
                ReturnTime = flight.ArrivalTime,
                DurationTime = CalculateDurationTime(depTime, arrTime),
                DepartureDate = flight.DepartureDate,
                ArrivalDate = flight.ArrivalDate,

            };
        }

        private string GenerateAirlineBookingCode(string airlineIataCode)
        {
            const string chars = "0123456789";
            var random = new Random(Guid.NewGuid().GetHashCode());
            var randomCode = new char[4];

            for (int i = 0; i < 4; i++)
            {
                randomCode[i] = chars[random.Next(chars.Length)];
            }

            return $"{airlineIataCode.ToUpperInvariant()}-{new string(randomCode)}";
        }
        private string CalculateDurationTime(TimeSpan departureTime, TimeSpan arrivalTime)
        {

            if (arrivalTime < departureTime)
            {
                arrivalTime = arrivalTime.Add(TimeSpan.FromHours(24));
            }

            TimeSpan duration = arrivalTime - departureTime;


            return $"{(int)duration.TotalHours}h {duration.Minutes}m";
        }


    }
}
