using System.ComponentModel.DataAnnotations;

namespace BaseEntity.Entities
{
    public class FlightAmenity
    {
        [Key]
        public int Id { get; set; }
        public string? FlightNumber { get; set; } = string.Empty;
        public Flight? Flight { get; set; }

        public int AmenityId { get; set; }
        public Amenity? Amenity { get; set; }
    }
}
