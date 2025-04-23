using System.ComponentModel.DataAnnotations;

namespace BaseEntity.Dtos
{
    public class GetPassportDto
    {
        public int Id { get; set; }
        public string PassportNumber { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public DateTime ExpiryDate { get; set; }
    }
}
