using BaseEntity.Entities;

namespace ServerLibrary.Builder
{
    public interface ITicketDirectory
    {
        Ticket ConstructTicketAsync(Booking booking, FlightSegment segment, Flight flight, Airline airline);
    }
}
