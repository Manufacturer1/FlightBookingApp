using BaseEntity.Entities;
using BaseEntity.Responses;
using System.Linq.Expressions;

namespace ServerLibrary.Repositories.Interfaces
{
    public interface IPassengerRepository
    {
        Task<int> CreateAsync(Passenger passenger);
        Task<GeneralReponse> RemoveAsync(int id);
        Task<Passenger?> GetAsync(int id);
        Task<IEnumerable<Passenger>> GetAllAsync();
        Task<GeneralReponse> UpdateAsync(Passenger passenger);
        Task<Passenger?> FindAsync(Expression<Func<Passenger, bool>> expression);
    }
}
