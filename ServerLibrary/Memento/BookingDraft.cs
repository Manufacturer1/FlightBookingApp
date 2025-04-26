using BaseEntity.Dtos;

namespace ServerLibrary.Memento
{
    // Originator
    public class BookingDraft
    {
        public CreatePassengerDto? Passenger { get; set; }
        public CreatePassportDto? Passport { get; set; }
        public CreateContactDetailsDto? ContactDetails { get; set; }
        public string? PaymentIntentId { get; set; }
        public int? ActiveStep {  get; set; }
        public BookingDraftMemento Save()
        {
            var passenger = Passenger == null ? null : new CreatePassengerDto
            {
                Name = Passenger.Name,
                Surname = Passenger.Surname,
                BirthDay = Passenger.BirthDay,
                Nationality = Passenger.Nationality,
            };

            var passport = Passport == null ? null : new CreatePassportDto
            {
                PassportNumber = Passport.PassportNumber,
                Country = Passport.Country,
                ExpiryDate = Passport.ExpiryDate,
                
            };
            var contactDetails = ContactDetails == null ? null : new CreateContactDetailsDto
            {
                Surname = ContactDetails.Surname,
                Email = ContactDetails.Email,
                Name = ContactDetails.Name,
                PhoneNumber = ContactDetails.PhoneNumber,
            };
        

            string paymentIntentId = PaymentIntentId!;
            int activeStep = ActiveStep ?? 0;
 

            return new BookingDraftMemento(passport, passenger, contactDetails,paymentIntentId,activeStep);
        }

        public void Restore(BookingDraftMemento memento)
        {
            Passenger = memento.Passenger;
            Passport = memento.Passport;
            ContactDetails = memento.ContactDetails;
            PaymentIntentId = memento.PaymentIntentId;
        }
    }
}
