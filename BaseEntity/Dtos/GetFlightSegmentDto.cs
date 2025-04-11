namespace BaseEntity.Dtos
{
    public class GetFlightSegmentDto
    {
        public string FlightNumber { get; set; } = string.Empty;
        public int SegmentOrder { get; set; }
    }
}