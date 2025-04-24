using System.ComponentModel.DataAnnotations;

namespace BaseEntity.Dtos
{
    public class CreateContactDetailsDto
    {
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        [StringLength(100)]
        public string Surname { get; set; } = string.Empty;
        [StringLength(50)]
        public string Email { get; set; } = string.Empty;
        [StringLength(20)]
        public string PhoneNumber { get; set; } = string.Empty;
    }
}
