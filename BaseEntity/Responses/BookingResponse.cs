using BaseEntity.Dtos;

namespace BaseEntity.Responses
{
    public class BookingResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public List<GetTicketDto>? Tickets { get; set; }

        public BookingResponse(bool success, string message, List<GetTicketDto>? tickets)
        {
            Success = success;
            Message = message;
            Tickets = tickets;
        }
    }
}
