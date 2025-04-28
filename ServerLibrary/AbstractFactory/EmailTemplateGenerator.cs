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

        public string GenerateCreatedDiscountTemplate(Discount discount)
        {
            return $@"
        <html>
            <body style=""font-family: Arial, sans-serif; color: #333;"">
                <h1 style=""color: #e67e22;"">🎉 Special Discount Alert! 🎉</h1>
                
                <h2>{discount.Name}</h2>
                <p style=""font-size: 18px; font-weight: bold; color: #e74c3c;"">
                    Save {discount.Percentage}% on your next booking!
                </p>
                
                <div style=""background-color: #f8f9fa; padding: 15px; border-radius: 5px; margin: 15px 0;"">
                    <p style=""margin: 0;"">
                        {(discount.IsActive
                                    ? "✅ This offer is currently active!"
                                    : "⚠️ Note: This offer is not currently active")}
                    </p>
                </div>
                
                <p>Don't miss this opportunity to save on your next flight!</p>
                
            </body>
        </html>
    ";
        }

        public string GenerateRemovedDiscountTemplate(Discount discount)
        {
            return $@"
        <html>
            <body style=""font-family: Arial, sans-serif; color: #333;"">
                <h1 style=""color: #e74c3c;"">⚠️ Discount No Longer Available</h1>
                
                <h2>{discount.Name}</h2>
                <p style=""font-size: 16px;"">
                    We regret to inform you that the {discount.Percentage}% discount has been removed.
                </p>
                
                <div style=""background-color: #f8f9fa; padding: 15px; border-radius: 5px; margin: 15px 0;"">
                    <p style=""margin: 0;"">
                        This discount is no longer available for bookings.
                    </p>
                </div>
                
                
                <p style=""font-size: 14px; color: #7f8c8d;"">
                    Removed on: {DateTime.Now:dd MMMM yyyy}
                </p>
            </body>
        </html>
    ";
        }
        public string GenerateUpdatedDiscountTemplate(Discount discount)
        {
            return $@"
        <html>
            <body style=""font-family: Arial, sans-serif; color: #333;"">
                <h1 style=""color: #3498db;"">✏️ Discount Updated</h1>
                
                <h2>{discount.Name}</h2>
                <p style=""font-size: 18px; font-weight: bold;"">
                    Now offering {discount.Percentage}% savings!
                </p>
                
                <div style=""background-color: #f8f9fa; padding: 15px; border-radius: 5px; margin: 15px 0;"">
                    <p style=""margin: 0;"">
                        {(discount.IsActive
                                    ? "✅ This offer is currently active!"
                                    : "⏸️ Note: This offer is currently paused")}
                    </p>
                </div>
                
                <p>Book now to take advantage of this updated offer!</p>
                
                <p style=""font-size: 14px; color: #7f8c8d;"">
                    Last updated: {DateTime.Now:dd MMMM yyyy}
                </p>
            </body>
        </html>
    ";
        }
    }
}
