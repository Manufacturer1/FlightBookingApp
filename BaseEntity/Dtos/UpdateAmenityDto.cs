using System.ComponentModel.DataAnnotations;

namespace BaseEntity.Dtos
{
    public class UpdateAmenityDto
    {
        public string? Name { get; set; } 
        public string? Description { get; set; } 
        public string? AmenityIconUrl { get; set; } 
    }
}
