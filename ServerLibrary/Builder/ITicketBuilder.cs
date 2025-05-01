using BaseEntity.Dtos;
using BaseEntity.Entities;
using BaseEntity.Responses;

namespace ServerLibrary.Builder
{
    public interface ITicketBuilder
    {
        void SetBooking(Booking booking);
        void SetAirline(Airline airline);
        void SetFlightSegment(FlightSegment flightSegment);
        void SetFlight(Flight flight);
        Ticket BuildTicket();
        Task<GetTicketDto> BuildTicketDtoAsync(int ticketId);
    }
}
