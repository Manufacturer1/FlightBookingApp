using BaseEntity.Entities;
using BaseEntity.Responses;

namespace ServerLibrary.Repositories.Interfaces
{
    public interface IPassengerRepository
    {
        Task<int> CreateAsync(Passenger passenger);
        Task<GeneralReponse> RemoveAsync(int id);
        Task<Passenger?> GetAsync(int id);
        Task<IEnumerable<Passenger>> GetAllAsync();
        Task<GeneralReponse> UpdateAsync(Passenger passenger);
    }
}
