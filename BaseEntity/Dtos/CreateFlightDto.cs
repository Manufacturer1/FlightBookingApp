using BaseEntity.CustomValidations;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace BaseEntity.Dtos
{
    public class CreateFlightDto
    {
        [Required]
        public string? ClassType { get; set; } = string.Empty;

        [Required]
        public int PlaneId { get; set; }
        [Required]
        public int? OriginAirportId { get; set; }
        [Required]
        public int? DestinationAirportId { get; set; }

        [Required]
        public string TripType { get; set; } = string.Empty;
        [Required]
        public string Origin { get; set; } = string.Empty;
        [Required]
        public string Destination { get; set; } = string.Empty;
        [Required]
        public IFormFile? DestinationImage { get; set; }

        [Required]
        public int TotalSeats { get; set; }
        [Required]
        public int AvailableSeats { get; set; }
        [Required]
        public decimal BasePrice { get; set; }

        [Required]
        [DataType(DataType.Date, ErrorMessage = "Invalid date format")]
        public DateTime DepartureDate { get; set; } = DateTime.Now;

        [Required]
        [DataType(DataType.Date, ErrorMessage = "Invalid date format")]
        [ArrivalAfterDeparture]
        public DateTime ArrivalDate { get; set; } = DateTime.Now.AddDays(1);

        [Required(ErrorMessage = "Departure time is required.")]
        [RegularExpression(@"^([01]?[0-9]|2[0-3]):[0-5][0-9]$", ErrorMessage = "Time must be in HH:mm format.")]
        public string DepartureTime { get; set; } = string.Empty;

        [Required(ErrorMessage = "Arrival time is required.")]
        [RegularExpression(@"^([01]?[0-9]|2[0-3]):[0-5][0-9]]$", ErrorMessage = "Time must be in HH:mm format.")]
        public string ArrivalTime { get; set; } = string.Empty;
    }
}
