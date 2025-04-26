using BaseEntity.Entities;

namespace ServerLibrary.AbstractFactory
{
    public interface INotificationFactory
    {
        INotificationTemplateGenerator CreateTemplateGenerator();
        INotificationSender CreateSender();
    }
}
