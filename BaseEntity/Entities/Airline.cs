using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BaseEntity.Entities
{
    public class Airline
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string AirlineImageUrl { get; set; } = string.Empty;
        [Required]
        public string AirlineBgColor { get; set; } = "#0D78C9FF";

        [JsonIgnore]
        public ICollection<Flight>? Flights { get; set; } 

        public int BaggagePolicyId { get; set; }
        public BaggagePolicy? BaggagePolicy { get; set; }
    }
}
