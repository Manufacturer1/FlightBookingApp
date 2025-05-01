namespace BaseEntity.Dtos
{
    public class GetFlightSegmentDto
    {
        public string FlightNumber { get; set; } = string.Empty;
        public int SegmentOrder { get; set; }
        public bool IsReturnSegment { get; set; } = false;
    }
}