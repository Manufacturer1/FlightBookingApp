namespace BaseEntity.Entities
{
    public class Airline
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string AirlineImageUrl { get; set; } = string.Empty;
        public string AirlineBgColor { get; set; } = "#0D78C9FF";
        public ICollection<Itinerary>? Itineraries { get; set; }
        public int BaggagePolicyId { get; set; }
        public BaggagePolicy? BaggagePolicy { get; set; }

    }
}
