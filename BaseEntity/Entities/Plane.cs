namespace BaseEntity.Entities
{
    public class Plane
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;  
        public string Model { get; set; } = string.Empty;
        public int SeatPitch { get; set; } = 29;
        public string SeatLayout { get; set; } = "3-3";
        public ICollection<Flight>? Flights { get; set; } 

    }
}
