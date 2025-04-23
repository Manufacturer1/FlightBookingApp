using System.ComponentModel.DataAnnotations;

namespace BaseEntity.Dtos
{
    public class GetBookingDto
    {
        public int Id { get; set; }
        public int PassengerId { get; set; }
        public int PassengerNumberSelected { get; set; }
        public int ItineraryId { get; set; }
        public DateTime BookingDate { get; set; }
        public string PaymentIntentId { get; set; } = string.Empty;
    }
}
