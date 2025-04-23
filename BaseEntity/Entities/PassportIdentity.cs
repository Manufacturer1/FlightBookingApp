using System.ComponentModel.DataAnnotations;

namespace BaseEntity.Entities
{
    public class PassportIdentity
    {
        public int Id { get; set; }
        [StringLength(100)]
        public string PassportNumber { get; set; } = string.Empty;
        [StringLength(100)]
        public string Country {  get; set; } = string.Empty;
        public DateTime ExpiryDate { get; set; }

    }
}
