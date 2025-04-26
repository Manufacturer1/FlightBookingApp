using AutoMapper;
using BaseEntity.Dtos;
using BaseEntity.Entities;
using BaseEntity.Responses;
using ServerLibrary.Repositories.Interfaces;
using ServerLibrary.Services.Interfaces;

namespace ServerLibrary.Services.Implementations
{
    public class TicketService : ITicketService
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IBookingRepository _bookingRepository;
        private readonly IItineraryRepository _itineraryRepository;
        private readonly IFlightRepository _flightRepository;
        private readonly IAirlineRepository _airlineRepository;
        private readonly IMapper _mapper;
        public TicketService(ITicketRepository ticketRepository,
            IBookingRepository bookingRepository,
            IItineraryRepository itineraryRepository,
            IFlightRepository flightRepository,
            IAirlineRepository airlineRepository,
            IMapper mapper)
        {
            _bookingRepository = bookingRepository;
            _ticketRepository = ticketRepository;
            _itineraryRepository = itineraryRepository;
            _flightRepository = flightRepository;
            _airlineRepository = airlineRepository;
            _mapper = mapper;
        }
        public async Task<(GeneralReponse Response,List<GetTicketDto>? tickets)> GenerateTicketAsync(CreateTicketDto createTicket)
        {
            var booking = await _bookingRepository.GetAsync(createTicket.BookingId);
            if (booking == null) return (new GeneralReponse(false, "Booking was not found."),null);

            var itinerary = await _itineraryRepository.GetByIdAsync(booking.ItineraryId);
            if (itinerary == null) return (new GeneralReponse(false, "Itinerary was not found."),null);

            var airline = await _airlineRepository.GetByIdAsync(itinerary.AirlineId);
            if (airline == null) return (new GeneralReponse(false, "Airline is not found."),null);

            var tickets = new List<GetTicketDto>();
            try
            {
                foreach(var segment in itinerary.Segments!)
                {
                    var flight = await _flightRepository.GetByFlightNumberAsync(segment.FlightNumber);
                    if (flight == null) return (new GeneralReponse(false, "Flight is not found."),null);

                    var generatedAirlineCode = GenerateAirlineBookingCode(airline.IataCode);
           
                    if(!TimeSpan.TryParse(segment.Flight!.DepartureTime,out TimeSpan departureTime))
                        return (new GeneralReponse(false, $"Invalid departure time format: ${segment.Flight.DepartureTime}"),null);

                    DateTime checkInDate;
                
                    DateTime departureDateTime = flight.DepartureDate.Date + departureTime;
                    checkInDate = departureDateTime.AddMinutes(-10);

                    var ticket = new Ticket
                    {
                        BookingId = createTicket.BookingId,
                        AirlineBookingCode = generatedAirlineCode,
                        IssueDate = DateTime.Now,
                        CheckInDate = checkInDate,
                        FlightNumber = flight.FlightNumber,
                        PaymentIntentId = booking.PaymentIntentId

                    };
                    var (ticketResponse,ticketId) = await _ticketRepository.CreateAsync(ticket);
                    if (!ticketResponse.Flag || !ticketId.HasValue)
                        return (ticketResponse, null);

                    try
                    {
                        var getTicket = await GetTicketAsync(ticketId.Value);
                        tickets.Add(getTicket);
                    }
                    catch(Exception ex)
                    {
                        return (new GeneralReponse(false,ex.Message),null);
                    }
                
                }
                return (new GeneralReponse(true, "Tickets were generated successfuly."),tickets);
            }
            catch(Exception ex)
            {
                return (new GeneralReponse(false, ex.Message),null);
            }

        }
        public async Task<GetTicketDto> GetTicketAsync(int id)
        {
            var ticket = await _ticketRepository.GetAsync(id);
            if (ticket == null) throw new Exception("Ticket was not found.");

            var booking = await _bookingRepository.GetAsync(ticket.BookingId);
            var itinerary = await _itineraryRepository.GetByIdAsync(booking!.ItineraryId);

            var airline = await _airlineRepository.GetByIdAsync(itinerary!.AirlineId);
            if (airline == null) throw new Exception("Airline was not found.");

            var airlineDto = _mapper.Map<GetAirlineDto>(airline);

            var baggage = _mapper.Map<GetBaggageDto>(airline.BaggagePolicy);

            var flight = await _flightRepository.GetByFlightNumberAsync(ticket.FlightNumber);
            if (flight == null) throw new Exception("Flight was not found.");

            if (!TimeSpan.TryParse(flight.DepartureTime, out var departureTime) || !TimeSpan.TryParse(flight.ArrivalTime, out var arrivalTime))
                throw new Exception("Departure or arrival time is not in correct format");


            var generatedTicket = new GetTicketDto
            {
                Id = id,
                BookingId = ticket.BookingId,
                CheckInDate = ticket.CheckInDate,
                IssueDate = ticket.IssueDate,
                AirlineBookingCode = ticket.AirlineBookingCode,
                Airline = airlineDto,
                Baggage = baggage,
                PassengerName = booking.Passenger!.Name,
                PassengerSurname = booking.Passenger!.Surname,
                Origin = flight.Origin,
                Destination = flight.Destination,
                ClassType = flight.ClassType!,
                DepartureTime = flight.DepartureTime,
                ReturnTime = flight.ArrivalTime,
                DurationTime = CalculateDurationTime(departureTime,arrivalTime),
                DepartureDate = flight.DepartureDate,
                ArrivalDate = flight.ArrivalDate,
            };


            return generatedTicket;
        }
        public async Task<IEnumerable<GetTicketDto>?> GetTicketByPaymentIntentIdAsync(string paymentIntentId)
        {
            var tickets = await _ticketRepository.GetByPredicate(x => x.PaymentIntentId == paymentIntentId);
            if(tickets == null ) return null;

            var getTickets = new List<GetTicketDto>();

            foreach(var ticket in tickets)
            {
                var generatedTicket = await GetTicketAsync(ticket.Id);
                if (generatedTicket == null) return null;

                getTickets.Add(generatedTicket);
            }

            return getTickets;
        }
        public async Task<GeneralReponse> RemoveTicketAsync(int id)
        {
            return await _ticketRepository.DeleteAsync(id);
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
