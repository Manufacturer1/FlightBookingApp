using BaseEntity.Dtos;

namespace ServerLibrary.Validators
{
    public class PassportValidator : IPassportValidator
    {
        public ValidationResult Validate(CreatePassportDto passport)
        {
            if (passport == null)
                return new ValidationResult(false, "Passport details are required");

            if (string.IsNullOrWhiteSpace(passport.PassportNumber))
                return new ValidationResult(false, "Passport number is required");
            if (string.IsNullOrEmpty(passport.Country))
                return new ValidationResult(false, "Passport country is required.");

            if (!passport.ExpiryDate.HasValue || passport.ExpiryDate.Value.Date <= DateTime.Now.Date)
                return new ValidationResult(false, "Passport has expired");

            return new ValidationResult(true, null!);
        }
    }
}
