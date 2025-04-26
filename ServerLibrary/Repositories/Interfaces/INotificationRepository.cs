using BaseEntity.Entities;
using BaseEntity.Responses;

namespace ServerLibrary.Repositories.Interfaces
{
    public interface INotificationRepository
    {
        Task<GeneralReponse> CreateAsync(Notification notification);
        Task<GeneralReponse> RemoveAsync(int id);
        Task<Notification?> GetAsync(int id);
        Task<IEnumerable<Notification>> GetAllAsync();
    }
}
