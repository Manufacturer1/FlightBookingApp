namespace BaseEntity.Dtos
{
    public class GetItineraryDto
    {
        public int Id { get; set; }
        public int AirlineId { get; set; }
        public string Origin { get; set; } = string.Empty;
        public string Destination { get; set; } = string.Empty;
        public ICollection<GetFlightSegmentDto>? Segments { get; set; }
    }
}
