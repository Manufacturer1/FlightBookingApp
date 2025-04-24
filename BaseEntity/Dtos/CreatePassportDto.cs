using System.ComponentModel.DataAnnotations;

namespace BaseEntity.Dtos
{
    public class CreatePassportDto
    {
        [StringLength(100)]
        public string? PassportNumber { get; set; }
        [StringLength(100)]
        public string? Country { get; set; }
        [DataType(DataType.Date)]
        public DateTime? ExpiryDate { get; set; }
    }
}
