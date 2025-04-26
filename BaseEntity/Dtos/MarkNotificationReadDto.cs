using System.ComponentModel.DataAnnotations;

namespace BaseEntity.Dtos
{
    public class MarkNotificationReadDto
    {
        [Required]
        public bool IsRead { get; set; }
    }
}
