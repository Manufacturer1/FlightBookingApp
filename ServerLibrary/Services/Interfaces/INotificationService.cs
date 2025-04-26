using BaseEntity.Dtos;
using BaseEntity.Responses;

namespace ServerLibrary.Services.Interfaces
{
    public interface INotificationService
    {
        Task<GeneralReponse> SendBookingConfirmation(SendNotificationDto sendNotification);
        Task<IEnumerable<GetNotificationDto>> GetAllInAppNotifications();
        Task<IEnumerable<GetNotificationDto>?> GetNotificationsByPassengerEmailAsync(string email);
        Task<GeneralReponse> UpdateAsync(MarkNotificationReadDto mark, int notificationId);
    }
}
