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
        [Required]
        public DateTime TravelDate { get; set; }

        public ICollection<FlightSegmentRequest>? Segments { get; set; }
    }
}
