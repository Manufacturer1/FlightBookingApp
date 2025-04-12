namespace BaseEntity.Dtos
{
    public class GetAirlineDto
    {
        public int Id { get; set; }
        public int BaggagePolicyId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string AirlineImageUrl { get; set; } = string.Empty;
        public string AirlineBgColor { get; set; } = "#0D78C9FF";
        public IEnumerable<GetItineraryDto>? Itineraries { get; set; }
       

    }
}
