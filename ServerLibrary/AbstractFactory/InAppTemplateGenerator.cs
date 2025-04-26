using System.Text;
using BaseEntity.Entities;

namespace ServerLibrary.AbstractFactory
{
    public class InAppTemplateGenerator : INotificationTemplateGenerator
    {
        public string GenerateBookingConfirmation(Booking booking)
        {
            var sb = new StringBuilder();

            sb.AppendLine("Booking Confirmed! ✈️");
            sb.AppendLine();
            sb.AppendLine($"Passenger: {booking.Passenger?.Name ?? "Unknown"} {booking.Passenger?.Surname ?? "Unknown"}");
            sb.AppendLine($"Flight: {booking.Itinerary?.Id ?? 0}");
            sb.AppendLine($"From: {booking.Itinerary?.Origin ?? "Unknown"} → To: {booking.Itinerary?.Destination ?? "Unknown"}");
            sb.AppendLine();
            sb.AppendLine($"Departure: {booking.Itinerary?.DepartureDate:dd.MM.yyyy} at {booking.Itinerary?.DepartureTime ?? "Unknown"}");
            sb.AppendLine($"Arrival: {booking.Itinerary?.ArrivalDate:dd.MM.yyyy} at {booking.Itinerary?.ArrivalTime ?? "Unknown"}");
            sb.AppendLine();
            sb.AppendLine($"Airline: {booking.Itinerary?.Airline?.Name ?? "N/A"}");
            sb.AppendLine($"Baggage Allowance: {booking.Itinerary?.Airline?.BaggagePolicy?.CheckedWeightLimitKg ?? 0} kg");
            sb.AppendLine($"Flight Duration: {booking.Itinerary?.DurationTime ?? "Unknown"}");
            sb.AppendLine();
            sb.AppendLine("Thank you for choosing us! 🚀");

            return sb.ToString();
        }
    }
}
