using System.Text.Json.Serialization;

namespace BaseEntity.Entities
{
    public class Plane
    {
        public int Id { get; set; }
        [Rquired]
        public string Name { get; set; } = string.Empty;
        [Rquired]   
        public string Model { get; set; } = string.Empty;

        [JsonIgnore]
        public ICollection<Flight>? Flights { get; set; } 

    }
}
