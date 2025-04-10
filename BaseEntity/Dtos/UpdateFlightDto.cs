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
        public int AirlineId { get; set; }
        [Required]
        public int PlaneId { get; set; }
        public string? ClassType { get; set; } = string.Empty;
        public string TripType { get; set; } = string.Empty;
        public string Origin { get; set; } = string.Empty;
        public string Destination { get; set; } = string.Empty;
        public IFormFile? DestinationImage { get; set; }
        public int TotalSeats { get; set; }
        public int AvailableSeats { get; set; }
        public decimal BasePrice { get; set; }

        [DataType(DataType.Date, ErrorMessage = "Invalid date format")]
        public DateTime DepartureDate { get; set; } = DateTime.Now;

        [DataType(DataType.Date, ErrorMessage = "Invalid date format")]
        [ArrivalAfterDeparture]
        public DateTime ArrivalDate { get; set; } = DateTime.Now.AddDays(1);

        [RegularExpression(@"^([01]?[0-9]|2[0-3]):[0-5][0-9]:[0-5][0-9]$", ErrorMessage = "Time must be in HH:mm:ss format.")]
        public string DepartureTime { get; set; } = string.Empty;
        [RegularExpression(@"^([01]?[0-9]|2[0-3]):[0-5][0-9]:[0-5][0-9]$", ErrorMessage = "Time must be in HH:mm:ss format.")]
        public string ArrivalTime { get; set; } = string.Empty;
    }
}
