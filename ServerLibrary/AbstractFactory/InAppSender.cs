using BaseEntity.Entities;
using ServerLibrary.Repositories.Interfaces;

namespace ServerLibrary.AbstractFactory
{
    public class InAppSender : INotificationSender
    {
        private readonly INotificationRepository _notificationRepository;
        public InAppSender(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }
        public async Task Send(Notification notification)
        {
            await _notificationRepository.CreateAsync(notification);
        }
    }
}
