using System.ComponentModel.DataAnnotations;

namespace BaseEntity.Dtos
{
    public class CreatePlaneDto
    {
        [Required]
        public string Name { get; set; } = "Airbus";
        [Required]
        public string Model { get; set; } = "A320";
        [Required]
        public string SeatLayout { get; set; } = "3-3";
        [Required]
        public int SeatPitch { get; set; } = 29;
    }
}
