using BaseEntity.Dtos;

namespace ServerLibrary.Memento
{
    // Memento
    public class BookingDraftMemento
    {
        public CreatePassengerDto? Passenger { get; init; }
        public CreatePassportDto? Passport { get; init; }
        public CreateContactDetailsDto? ContactDetails { get; init; }

        public BookingDraftMemento(CreatePassportDto? Passport, CreatePassengerDto? Passenger, CreateContactDetailsDto? ContactDetails)
        {
            this.Passenger = Passenger;
            this.Passport = Passport;
            this.ContactDetails = ContactDetails;
        }


    }
}
