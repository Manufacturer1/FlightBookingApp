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
        public DateTime DepartureDate { get; set; } = DateTime.Now;
        public DateTime ArrivalDate { get; set; } = DateTime.Now.AddDays(1);
        public string DepartureTime { get; set; } = string.Empty;
        public string ArrivalTime { get; set; } = string.Empty;
        public int? PlaneId { get; set; }
        public Plane? Plane { get; set; } 

        public ICollection<FlightSegment>? Segments { get; set; }
    }
}
