using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace BaseEntity.Dtos
{
    public class AmenityImageUploaderDto
    {
        [Required]
        public IFormFile? AmenityIcon { get; set; }
    }
}
