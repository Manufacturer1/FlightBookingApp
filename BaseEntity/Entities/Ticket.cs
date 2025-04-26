using System.ComponentModel.DataAnnotations;

namespace BaseEntity.Entities
{
    public class Ticket
    {
        public int Id { get; set; }
        public int BookingId { get; set; }
        public string FlightNumber { get; set; } = string.Empty;
        [StringLength(30)]
        public string AirlineBookingCode { get; set; } = string.Empty;
        [StringLength(30)]
        public string PaymentIntentId {  get; set; } = string.Empty;
        public DateTime IssueDate { get; set; } = DateTime.Now;
        public DateTime CheckInDate {  get; set; } = DateTime.Now;
        public Booking? Booking { get; set; }
        public Flight? Flight { get; set; }

    }
}
