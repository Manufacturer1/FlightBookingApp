using BaseEntity.Entities;

namespace ServerLibrary.AbstractFactory
{
    public interface INotificationSender
    {
        Task Send(Notification notification);
    }
}
