using System.ComponentModel.DataAnnotations;

namespace BaseEntity.Dtos
{
    public class SendNotificationDto
    {
        [Required]
        public int BookingId { get; set; }
    }
}
