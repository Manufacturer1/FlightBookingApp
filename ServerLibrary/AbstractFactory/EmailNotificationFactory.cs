using ServerLibrary.Repositories.Interfaces;

namespace ServerLibrary.AbstractFactory
{
    public class EmailNotificationFactory : INotificationFactory
    {
        private readonly IPassengerRepository _passengerRepository;


        public EmailNotificationFactory(IPassengerRepository passengerRepository)
        {
            _passengerRepository = passengerRepository;
        }

        public INotificationSender CreateSender()
             => new EmailSender(_passengerRepository);

        public INotificationTemplateGenerator CreateTemplateGenerator()
            => new EmailTemplateGenerator();
    }
}
