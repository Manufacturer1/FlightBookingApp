using System.ComponentModel.DataAnnotations;

namespace BaseEntity.Dtos
{
    public class CreateBookingDto
    {
        [Required]
        public int PassengerNumberSelected { get; set; } = 1;
        [Required]
        public int ItineraryId { get; set; }
        [Required]
        public string? PaymentIntentId { get; set; }
        public DateTime BookingDate { get; set; } = DateTime.Now;

    }
}
