using System.ComponentModel.DataAnnotations;

namespace BaseEntity.Dtos
{
    public class FlightSegmentRequest
    {
        [Required]
        public string FlightNumber { get; set; } = string.Empty;
        [Required]
        public bool IsReturnSegment { get; set; } = false;
    }
}
