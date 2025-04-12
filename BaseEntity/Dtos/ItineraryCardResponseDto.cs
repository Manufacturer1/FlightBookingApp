namespace BaseEntity.Dtos
{
    public class ItineraryCardResponseDto
    {
        public GetItineraryDto Itinerary { get; set; } = new();
        public List<FlightCardResponseDto> Flights { get; set; } = new();
    }
}
