using BaseEntity.Dtos;

namespace ServerLibrary.Validators
{
    public interface IPassportValidator
    {
        ValidationResult Validate(CreatePassportDto passport);
    }
}
