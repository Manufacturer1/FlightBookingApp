using BaseEntity.Dtos;

namespace ServerLibrary.Validators
{
    public interface IContactValidator
    {
        ValidationResult Validate(CreateContactDetailsDto contact);
    }
}
