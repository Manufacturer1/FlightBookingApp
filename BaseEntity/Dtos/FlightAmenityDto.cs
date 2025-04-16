using System.ComponentModel.DataAnnotations;

namespace BaseEntity.Dtos
{
    public class FlightAmenityDto
    {
        [Required]
        public string FlightNumber { get; set; } = string.Empty;
    }
}
