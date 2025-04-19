using System.ComponentModel.DataAnnotations;

namespace BaseEntity.Dtos
{
    public class CreateDiscountDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public decimal Percentage { get; set; }
        [Required]
        public bool IsActive { get; set; } = true;
    }
}
