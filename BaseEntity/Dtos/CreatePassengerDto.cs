using System.ComponentModel.DataAnnotations;

namespace BaseEntity.Dtos
{
    public class CreatePassengerDto
    {
        [Required]  
        public int PassportIdentityId { get; set; }
        [Required]
        public int ContactDetailsId { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        [Required]
        [StringLength(100)]
        public string Surname { get; set; } = string.Empty;
        [Required]
        [DataType(DataType.Date)]
        public DateTime BirthDay { get; set; }
        [Required]
        [StringLength(100)]
        public string Nationality { get; set; } = string.Empty;
    }
}
