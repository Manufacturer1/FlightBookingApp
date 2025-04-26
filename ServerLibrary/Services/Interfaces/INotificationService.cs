using BaseEntity.Dtos;
using BaseEntity.Responses;

namespace ServerLibrary.Services.Interfaces
{
    public interface INotificationService
    {
        Task<GeneralReponse> SendBookingConfirmation(SendNotificationDto sendNotification);
        Task<IEnumerable<GetNotificationDto>> GetAllInAppNotifications();
    }
}
