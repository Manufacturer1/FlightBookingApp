namespace BaseEntity.Dtos
{
    public class GetAmenityDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string AmenityIconUrl { get; set; } = string.Empty;
        public ICollection<FlightAmenityDto>? FlightAmenities { get; set; }
    }
}
