using System.ComponentModel.DataAnnotations.Schema;

namespace BaseEntity.Entities
{
    public class Itinerary
    {
        public int Id { get; set; }
        public int AirlineId { get; set; }
        public string Origin { get; set; } = string.Empty;
        public string Destination { get; set; } = string.Empty;
        public ICollection<FlightSegment>? Segments { get; set; }
        public Airline? Airline { get; set; }

        [NotMapped]
        public bool IsDirect => Segments?.Count == 1;
    }
}
