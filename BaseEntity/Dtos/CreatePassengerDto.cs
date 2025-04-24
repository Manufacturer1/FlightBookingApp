using System.ComponentModel.DataAnnotations;

namespace BaseEntity.Dtos
{
    public class CreatePassengerDto
    {
       
       
        [StringLength(100)]
        public string? Name { get; set; }
    
        [StringLength(100)]
        public string? Surname { get; set; }
      
        [DataType(DataType.Date)]
        public DateTime BirthDay { get; set; }
 
        [StringLength(100)]
        public string? Nationality { get; set; }
    }
}
