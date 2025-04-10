using BaseEntity.Entities;

namespace BaseEntity.Dtos
{
    public class CreatePlaneDto
    {
        [Rquired]
        public string Name { get; set; } = string.Empty;
        [Rquired]
        public string Model { get; set; } = string.Empty;
    }
}
