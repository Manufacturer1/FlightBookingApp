using System.ComponentModel.DataAnnotations;

namespace BaseEntity.Dtos
{
    public class CreateTicketDto
    {
        [Required]
        public int BookingId { get; set; }
    }
}
