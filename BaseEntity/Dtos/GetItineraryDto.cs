using System.ComponentModel.DataAnnotations.Schema;

namespace BaseEntity.Dtos
{
    public class GetItineraryDto
    {
        public int Id { get; set; }
        public int AirlineId { get; set; }
        public string Origin { get; set; } = string.Empty;
        public string Destination { get; set; } = string.Empty;
        public string? StopTime { get; set; } = string.Empty;
        public decimal TotalPrice { get; set; }
        public decimal CalculatedPrice { get; set; }
        public DateTime DepartureDate { get; set; }
        public DateTime ArrivalDate { get; set; }
        public DateTime ReturnSegmentDate { get; set; }
        public string DepartureTime { get; set; } = string.Empty;
        public string ArrivalTime { get; set; } = string.Empty;
        public string DurationTime { get; set; } = string.Empty;
        public bool HasStops { get; set; }
        public bool HasReturnSegments { get; set; }
        public ICollection<GetFlightSegmentDto>? Segments { get; set; }


    }
}
