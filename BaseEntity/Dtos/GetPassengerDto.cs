using System.ComponentModel.DataAnnotations;

namespace BaseEntity.Dtos
{
    public class GetPassengerDto
    {
        public int Id { get; set; }
        public int PassportIdentityId { get; set; }
        public int ContactDetailsId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public DateTime BirthDay { get; set; }
        public string Nationality { get; set; } = string.Empty;
        public GetContactDetailsDto? ContactDetails { get; set; }
        public GetPassportDto? Passport { get; set; }
    }
}
