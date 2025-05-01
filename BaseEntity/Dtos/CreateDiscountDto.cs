using System.ComponentModel.DataAnnotations;

namespace BaseEntity.Dtos
{
    public class CreateDiscountDto
    {
        public string Name { get; set; } = string.Empty;
        public decimal Percentage { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
