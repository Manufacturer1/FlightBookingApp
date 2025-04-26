using BaseEntity.Entities;
using BaseEntity.Responses;
using System.Linq.Expressions;

namespace ServerLibrary.Repositories.Interfaces
{
    public interface INotificationRepository
    {
        Task<GeneralReponse> CreateAsync(Notification notification);
        Task<GeneralReponse> RemoveAsync(int id);
        Task<Notification?> GetAsync(int id);
        Task<IEnumerable<Notification>> GetAllAsync();
        Task<IEnumerable<Notification>> FindAsync(Expression<Func<Notification, bool>> predicate);
        Task<GeneralReponse> UpdateAsync(Notification notification);
    }
}
