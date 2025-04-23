using System.ComponentModel.DataAnnotations;

namespace BaseEntity.Entities
{
    public class Booking
    {
        [Key]
        public int Id { get; set; }
        public int PassengerId { get; set; }
        public int PassengerNumberSelected { get; set; }
        public int ItineraryId { get; set; }
        public DateTime BookingDate { get; set; }
        [StringLength(200)]
        public string PaymentIntentId { get; set; } = string.Empty;
        public Itinerary? Itinerary { get; set; }
        public Passenger? Passenger { get; set; }
    }
}
