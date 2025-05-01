using BaseEntity.Entities;

namespace ServerLibrary.Builder
{
    public class TicketDirectory : ITicketDirectory
    {
        private readonly ITicketBuilder _builder;
        public TicketDirectory(ITicketBuilder builder)
        {
            _builder = builder;            
        }
        public Ticket ConstructTicketAsync(Booking booking, FlightSegment segment, Flight flight, Airline airline)
        {
            _builder.SetAirline(airline);
            _builder.SetBooking(booking);
            _builder.SetFlight(flight);
            _builder.SetFlightSegment(segment);

            return _builder.BuildTicket();
        }
    }
}
