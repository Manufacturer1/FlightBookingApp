namespace BaseEntity.Dtos
{
    public class GetPlaneDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public int SeatPitch { get; set; }
        public string SeatLayout {  get; set; } = string.Empty;
    }
}
