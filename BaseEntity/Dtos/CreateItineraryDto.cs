using System.ComponentModel.DataAnnotations;

namespace BaseEntity.Dtos
{
    public class CreateItineraryDto
    {
        [Required]
        public int AirlineId { get; set; }
        [Required]
        public string Origin { get; set; } = string.Empty;
        [Required]
        public string Destination { get; set; } = string.Empty;
        [RegularExpression(@"^([01]?[0-9]|2[0-3]):[0-5][0-9]$", ErrorMessage = "Time must be in HH:mm format.")]
        public string? StopTime { get; set; }

        [Required]
        public ICollection<FlightSegmentRequest>? Segments { get; set; }
    }
}
