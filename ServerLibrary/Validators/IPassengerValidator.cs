using BaseEntity.Dtos;

namespace ServerLibrary.Validators
{
    public interface IPassengerValidator
    {
        ValidationResult Validate(CreatePassengerDto passenger);
    }
}
