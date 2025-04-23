using System.ComponentModel.DataAnnotations;

namespace BaseEntity.Dtos
{
    public class CreateBookingDto
    {
        [Required]
        public int PassengerId { get; set; }
        [Required]
        public int PassengerNumberSelected { get; set; } = 1;
        [Required]
        public int ItineraryId { get; set; }
        public DateTime BookingDate { get; set; } = DateTime.Now;
        [Required]
        [StringLength(200)]
        public string PaymentIntentId { get; set; } = string.Empty;
    }
}
