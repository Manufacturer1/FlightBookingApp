using BaseEntity.Dtos;

namespace ServerLibrary.Validators
{
    public class PassengerValidator : IPassengerValidator
    {
        public ValidationResult Validate(CreatePassengerDto passenger)
        {
            if (passenger == null)
                return new ValidationResult(false, "Passenger details are required");

            if (string.IsNullOrWhiteSpace(passenger.Name))
                return new ValidationResult(false, "First name is required");

            if (string.IsNullOrWhiteSpace(passenger.Surname))
                return new ValidationResult(false, "Last name is required");

            if (passenger.BirthDay.Date >= DateTime.Now.Date)
                return new ValidationResult(false, "Invalid date of birth");

            return new ValidationResult(true, null!);
        }
    }
}
