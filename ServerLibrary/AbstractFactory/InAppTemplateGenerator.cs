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
        public string GenerateCreatedDiscountTemplate(Discount discount)
        {
            var sb = new StringBuilder();

            sb.AppendLine("🎉 Special Discount Available! 🎉");
            sb.AppendLine();
            sb.AppendLine($"Discount Name: {discount.Name}");
            sb.AppendLine($"You Save: {discount.Percentage}% OFF");
            sb.AppendLine();
            sb.AppendLine(discount.IsActive
                ? "This offer is currently active!"
                : "Note: This offer is not currently active");
            sb.AppendLine();
            sb.AppendLine("Hurry up and book your flight to take advantage of this great deal!");

            return sb.ToString();
        }

        public string GenerateRemovedDiscountTemplate(Discount discount)
        {
            var sb = new StringBuilder();

            sb.AppendLine("⚠️ Discount Removed");
            sb.AppendLine();
            sb.AppendLine($"Discount Name: {discount.Name}");
            sb.AppendLine($"Previous Savings: {discount.Percentage}% OFF");
            sb.AppendLine();
            sb.AppendLine("This discount is no longer available.");
            sb.AppendLine();
            sb.AppendLine("Check our app for other current promotions!");

            return sb.ToString();
        }

        public string GenerateUpdatedDiscountTemplate(Discount discount)
        {
            var sb = new StringBuilder();

            sb.AppendLine("✏️ Discount Updated");
            sb.AppendLine();
            sb.AppendLine($"Discount Name: {discount.Name}");
            sb.AppendLine($"New Savings: {discount.Percentage}% OFF");
            sb.AppendLine();
            sb.AppendLine(discount.IsActive
                ? "✅ This offer is currently active!"
                : "⏸️ Note: This offer is currently paused");
            sb.AppendLine();
            sb.AppendLine("Book now to take advantage of this updated offer!");

            return sb.ToString();
        }
    }
}
