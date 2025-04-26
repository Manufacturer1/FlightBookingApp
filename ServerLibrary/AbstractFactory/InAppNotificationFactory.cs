using ServerLibrary.Repositories.Interfaces;

namespace ServerLibrary.AbstractFactory
{
    public class InAppNotificationFactory : INotificationFactory
    {
        private readonly INotificationRepository _notificationRepository;

        public InAppNotificationFactory(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }
        public INotificationSender CreateSender()
            => new InAppSender(_notificationRepository);

        public INotificationTemplateGenerator CreateTemplateGenerator()
            => new InAppTemplateGenerator();
    }
}
