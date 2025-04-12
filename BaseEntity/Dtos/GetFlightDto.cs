namespace BaseEntity.Dtos
{
    public class GetFlightDto
    {
        public string FlightNumber { get; set; } = string.Empty;
        public int PlaneId { get; set; }
        public string ClassType { get; set; } = string.Empty;
        public string TripType {  get; set; } = string.Empty;
        public string Origin { get; set; } = string.Empty;
        public string Destination { get; set; } = string.Empty;
        public string DestinationImageUrl { get; set; } = string.Empty;
        public decimal BasePrice { get; set; }
        public string TimeIcon { get; set; } = string.Empty;
        public int TotalSeats { get; set; }
        public int AvailableSeats { get; set; }
        public DateTime DepartureDate { get; set; } = DateTime.Now;
        public DateTime ArrivalDate { get; set; } = DateTime.Now.AddDays(1);
        public string DepartureTime { get; set; } = new TimeSpan(09,20,0).ToString();   
        public string ArrivalTime { get; set; } = new TimeSpan(12,24,0).ToString();
    }
}
