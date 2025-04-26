using BaseEntity.Dtos;

namespace ServerLibrary.Memento
{
    // Memento
    public class BookingDraftMemento
    {
        public CreatePassengerDto? Passenger { get; init; }
        public CreatePassportDto? Passport { get; init; }
        public CreateContactDetailsDto? ContactDetails { get; init; }
        public string? PaymentIntentId { get; init; }
        public int? ActiveStep { get; init; }

        public BookingDraftMemento(CreatePassportDto? Passport,
            CreatePassengerDto? Passenger,
            CreateContactDetailsDto? ContactDetails, 
            string? PaymentIntentId, int? ActiveStep)
        {
            this.Passenger = Passenger;
            this.Passport = Passport;
            this.ContactDetails = ContactDetails;
            this.PaymentIntentId = PaymentIntentId;
            this.ActiveStep = ActiveStep;
        }


    }
}
