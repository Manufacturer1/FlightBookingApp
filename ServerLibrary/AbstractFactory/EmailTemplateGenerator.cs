using BaseEntity.Dtos;
using BaseEntity.Entities;

namespace ServerLibrary.AbstractFactory
{
    public class EmailTemplateGenerator : INotificationTemplateGenerator
    {
        public string GenerateBookingConfirmation(Booking booking)
            => $@"
                  <html>
                        <body style=""font-family: Arial, sans-serif; color: #333;"">
                            <h1>Flight {booking.Itinerary!.Id} - Booking Confirmed</h1>

                            <p><strong>Passenger:</strong> {booking.Passenger!.Name} {booking.Passenger.Surname}</p>
                            <p><strong>From:</strong> {booking.Itinerary.Origin} → <strong>To:</strong> {booking.Itinerary.Destination}</p>

                            <p><strong>Departure:</strong> {booking.Itinerary.DepartureDate:dd.MM.yyyy} at {booking.Itinerary.DepartureTime}</p>
                            <p><strong>Arrival:</strong> {booking.Itinerary.ArrivalDate:dd.MM.yyyy} at {booking.Itinerary.ArrivalTime}</p>

                            <p><strong>Airline:</strong> {booking.Itinerary.Airline?.Name?? "N/A"}</p>
                            <p><strong>Baggage:</strong> {booking.Itinerary?.Airline?.BaggagePolicy?.CheckedWeightLimitKg ?? 0} kg</p>

                            <p><strong>Duration:</strong> {booking.Itinerary!.DurationTime}</p>

                            <br/>
                            <p>Thank you for booking with us!</p>
                        </body>
                    </html>
                ";
    }
}
