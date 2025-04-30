using BaseEntity.Dtos;

namespace ServerLibrary.Validators
{
    public class ContactValidator : IContactValidator
    {
        public ValidationResult Validate(CreateContactDetailsDto contact)
        {
            if (contact == null)
                return new ValidationResult(false, "Contact details are required");

            if (string.IsNullOrWhiteSpace(contact.Email))
                return new ValidationResult(false, "Email is required");

            if (string.IsNullOrWhiteSpace(contact.PhoneNumber))
                return new ValidationResult(false, "Phone number is required");

            if (!IsValidEmail(contact.Email))
                return new ValidationResult(false, "Invalid email format");

            return new ValidationResult(true, null!);
        }
        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
