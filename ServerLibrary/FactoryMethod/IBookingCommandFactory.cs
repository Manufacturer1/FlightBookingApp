using BaseEntity.Dtos;
using ServerLibrary.Command;
using ServerLibrary.Memento;

namespace ServerLibrary.FactoryMethod
{
    public interface IBookingCommandFactory
    {
        ValidateItineraryCommand CreateValidateItineraryCommand(int itineraryId);
        ValidateBookingDraftCommand CreateValidateBookingDraftCommand(BookingDraftMemento draft);
        ValidateBookingDraftDetailsCommand CreateBookingDraftDetailsCommand(BookingDraftMemento draft);
        ProcessPassengerCommand CreateProcessPassangerCommand(BookingDraftMemento draft);
        UpdateSeatAvailabilityCommand CreateUpdateSeatCommand(ValidateItineraryCommand itineraryCmd, int passengerNumber);
        CreateBookingCommand CreateCreateBookingCommand(CreateBookingDto createBooking, ProcessPassengerCommand proccessPassangerCmd, BookingDraftMemento draft);
        GenerateTicketsCommand CreateGenerateTicketsCommand(CreateBookingCommand createBookingCmd);
    }
}
