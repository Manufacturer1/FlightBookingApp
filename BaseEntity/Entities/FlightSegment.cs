using System.ComponentModel.DataAnnotations;

namespace BaseEntity.Entities
{
    public class FlightSegment
    {
        public int Id { get; set; }
        public string FlightNumber { get; set; } = string.Empty;
        public Flight? Flight { get; set; }
        public int ItineraryId { get; set; }
        public Itinerary? Itinerary { get; set;}

        public int SegmentOrder { get; set; }
    }
}
