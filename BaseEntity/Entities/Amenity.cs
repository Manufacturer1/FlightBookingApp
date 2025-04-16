namespace BaseEntity.Entities
{
    public class Amenity
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string AmenityIconUrl { get; set; } = string.Empty;
        public virtual ICollection<FlightAmenity>? FlightAmenities { get; set; }
    }
}
