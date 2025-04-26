using BaseEntity.Dtos;
using BaseEntity.Entities;

namespace ServerLibrary.AbstractFactory
{
    public interface INotificationTemplateGenerator
    {
        string GenerateBookingConfirmation(Booking booking);

    }
}
