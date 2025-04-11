using System.ComponentModel.DataAnnotations;

namespace BaseEntity.Dtos
{
    public class FlightCardResponseDto
    {
        public string FlightNumber { get; set; } = string.Empty;
        public int PlaneId { get; set; } 
        public string TripType { get; set; } = string.Empty;
        public string Origin { get; set; } = string.Empty;
        public string Destination { get; set; } = string.Empty;
        public string DestinationImageUrl { get; set; } = string.Empty;
        public string TimeIcon { get; set; } = string.Empty;
        public decimal BasePrice { get; set; }
        public DateTime DepartureDate { get; set; }
        public DateTime ArrivalDate { get; set; }
        public string DepartureTime { get; set; } = string.Empty;
        public string ArrivalTime { get; set; } = string.Empty;
    }
}
