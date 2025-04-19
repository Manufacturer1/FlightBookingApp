using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace BaseEntity.Dtos
{
    public class CreateAmenityDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Description { get; set; } = string.Empty;
        [Required]
        public string AmenityIconUrl { get; set; } = string.Empty;

    }
}
