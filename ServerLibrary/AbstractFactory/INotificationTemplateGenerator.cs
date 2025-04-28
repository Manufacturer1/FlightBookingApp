using BaseEntity.Dtos;
using BaseEntity.Entities;

namespace ServerLibrary.AbstractFactory
{
    public interface INotificationTemplateGenerator
    {
        string GenerateBookingConfirmation(Booking booking);
        string GenerateCreatedDiscountTemplate(Discount discount);
        string GenerateRemovedDiscountTemplate(Discount discount);
        string GenerateUpdatedDiscountTemplate(Discount discount);
    }
}
