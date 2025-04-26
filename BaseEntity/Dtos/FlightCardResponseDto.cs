using System.ComponentModel.DataAnnotations;

namespace BaseEntity.Dtos
{
    public class FlightCardResponseDto
    {
        public string FlightNumber { get; set; } = string.Empty;
        public int PlaneId { get; set; }
        public int OriginAirportId { get; set; }
        public int DestinationAirportId { get; set; }
        public int AvailableSeats { get; set; }
        public int TotalSeats { get; set; }
        public string TripType { get; set; } = string.Empty;
        public string ClassType {  get; set; } = string.Empty;  
        public string Origin { get; set; } = string.Empty;
        public string Destination { get; set; } = string.Empty;
        public string DestinationImageUrl { get; set; } = string.Empty;
        public decimal BasePrice { get; set; }
        public string TimeIcon { get; set; } = string.Empty;
        public DateTime DepartureDate { get; set; }
        public DateTime ArrivalDate { get; set; }
        public string DepartureTime { get; set; } = string.Empty;
        public string ArrivalTime { get; set; } = string.Empty;
        public decimal FinalPrice {  get; set; } = decimal.Zero;

    }
}
