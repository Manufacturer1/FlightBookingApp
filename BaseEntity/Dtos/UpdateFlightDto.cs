using BaseEntity.CustomValidations;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace BaseEntity.Dtos
{
    public class UpdateFlightDto
    {
        [Required]
        public string FlightNumber { get; set; } = string.Empty;
        [Required]
        public int PlaneId { get; set; }
        [Required]
        public int? OriginAirportId { get; set; }
        [Required]
        public int? DestinationAirportId { get; set; }
        public string? ClassType { get; set; }
        public string? TripType { get; set; }
        public string? Origin { get; set; }
        public string? Destination { get; set; }
        public IFormFile? DestinationImage { get; set; }
        public int? TotalSeats { get; set; }
        public int? AvailableSeats { get; set; }
        public decimal? BasePrice { get; set; }


        [DataType(DataType.Date, ErrorMessage = "Invalid date format")]
        public DateTime? DepartureDate { get; set; } = null;

        [DataType(DataType.Date, ErrorMessage = "Invalid date format")]
        [ArrivalAfterDeparture]
        public DateTime? ArrivalDate { get; set; } = null;

        [RegularExpression(@"^([01]?[0-9]|2[0-3]):[0-5][0-9]:[0-5][0-9]$", ErrorMessage = "Time must be in HH:mm:ss format.")]
        public string? DepartureTime { get; set; }
        [RegularExpression(@"^([01]?[0-9]|2[0-3]):[0-5][0-9]:[0-5][0-9]$", ErrorMessage = "Time must be in HH:mm:ss format.")]
        public string? ArrivalTime { get; set; } 
    }
}
