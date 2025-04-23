using System.ComponentModel.DataAnnotations;

namespace BaseEntity.Dtos
{
    public class CreatePassportDto
    {
        [StringLength(100)]
        [Required]
        public string PassportNumber { get; set; } = string.Empty;
        [Required]

        [StringLength(100)]
        public string Country { get; set; } = string.Empty;
        [Required]
        [DataType(DataType.Date)]
        public DateTime ExpiryDate { get; set; }
    }
}
