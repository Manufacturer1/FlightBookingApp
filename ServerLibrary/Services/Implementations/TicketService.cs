using AutoMapper;
using BaseEntity.Dtos;
using BaseEntity.Entities;
using BaseEntity.Responses;
using ServerLibrary.Builder;
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

        private readonly ITicketBuilder _ticketBuilder;
        private readonly ITicketDirectory _ticketDirectory;

        public TicketService(ITicketRepository ticketRepository,
            IBookingRepository bookingRepository,
            IItineraryRepository itineraryRepository,
            IFlightRepository flightRepository,
            IAirlineRepository airlineRepository,
            ITicketBuilder ticketBuilder,
            ITicketDirectory ticketDirectory)
        {
            _bookingRepository = bookingRepository;
            _ticketRepository = ticketRepository;
            _itineraryRepository = itineraryRepository;
            _flightRepository = flightRepository;
            _airlineRepository = airlineRepository;
            _ticketBuilder = ticketBuilder;
            _ticketDirectory = ticketDirectory;
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

                    var ticket = _ticketDirectory.ConstructTicketAsync(booking,segment,flight,airline);
                    
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
           var generatedTicket = await _ticketBuilder.BuildTicketDtoAsync(id);

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

    }
}
