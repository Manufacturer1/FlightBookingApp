namespace BaseEntity.Dtos
{
    public class GetTicketDto
    {
        public int Id { get; set; }
        public int BookingId { get; set; }
        public string FlightNumber { get; set; } = string.Empty;
        public string AirlineBookingCode { get; set; } = string.Empty;
        public DateTime IssueDate { get; set; } = DateTime.Now;
        public DateTime CheckInDate { get; set; } = DateTime.Now;
        public GetAirlineDto? Airline { get; set; }
        public GetBaggageDto? Baggage { get; set; }
        public string PassengerName { get; set; } = string.Empty;
        public string PassengerSurname { get; set; } = string.Empty;
        public string Origin { get; set; } = string.Empty;
        public string Destination {  get; set; } = string.Empty;
        public string ClassType { get; set; } = string.Empty;
        public string DepartureTime { get; set; } = string.Empty;
        public string ReturnTime { get; set; } = string.Empty;
        public string DurationTime { get; set; } = string.Empty;
        public DateTime DepartureDate {  get; set; }
        public DateTime ArrivalDate { get; set; }
    
       
    }
}
