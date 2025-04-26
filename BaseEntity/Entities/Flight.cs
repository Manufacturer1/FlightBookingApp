using BaseEntity.CustomValidations;
using System.ComponentModel.DataAnnotations;

namespace BaseEntity.Entities
{
    public class Flight
    {
        [Key]
        public string FlightNumber { get; set; } = string.Empty;
     
        public string? ClassType { get; set; } = string.Empty;
        public string TripType {  get; set; } = string.Empty;
        public string Origin { get; set; } = string.Empty;
        public string Destination {  get; set; } = string.Empty;
        public string DestinationImageUrl { get; set; } = string.Empty;
        public string TimeIcon { get; set; } = string.Empty;
        public int TotalSeats { get; set; } 
        public int AvailableSeats { get; set; }
        public decimal BasePrice { get; set; }

        public int? OriginAirportId { get; set; }
        public int? DestinationAirportId { get; set; }
        [Required]
        [DataType(DataType.Date, ErrorMessage = "Invalid date format")]
        public DateTime DepartureDate { get; set; } = DateTime.Now;
        [Required]
        [DataType(DataType.Date, ErrorMessage = "Invalid date format")]
        [ArrivalAfterDeparture]
        public DateTime ArrivalDate { get; set; } = DateTime.Now.AddDays(1);
        [Required(ErrorMessage = "Departure time is required.")]
        [RegularExpression(@"^([01]?[0-9]|2[0-3]):[0-5][0-9]:[0-5][0-9]$", ErrorMessage = "Time must be in HH:mm:ss format.")]
        public string DepartureTime { get; set; } = string.Empty;
        [Required(ErrorMessage = "Arrival time is required.")]
        [RegularExpression(@"^([01]?[0-9]|2[0-3]):[0-5][0-9]:[0-5][0-9]$", ErrorMessage = "Time must be in HH:mm:ss format.")]
        public string ArrivalTime { get; set; } = string.Empty;
        public int? PlaneId { get; set; }
        public Plane? Plane { get; set; } 

        public ICollection<FlightSegment>? Segments { get; set; }

        public Airport? OriginAirport { get; set; }
        public Airport? DestinationAirport { get; set; }
        public virtual ICollection<FlightAmenity>? FlightAmenities { get; set; }
        public virtual ICollection<Ticket>? Tickets { get; set; }

    }
}
