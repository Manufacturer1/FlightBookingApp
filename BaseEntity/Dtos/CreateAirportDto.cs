using System.ComponentModel.DataAnnotations;

namespace BaseEntity.Dtos
{
    public class CreateAirportDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        [StringLength(3, MinimumLength = 3)]
        [RegularExpression(@"^[A-Z]{3}$")]
        public string Code { get; set; } = string.Empty;
    }
}
