using BaseEntity.Entities;
using BaseEntity.Responses;

namespace ServerLibrary.Repositories.Interfaces
{
    public interface IPassengerRepository
    {
        Task<GeneralReponse> CreateAsync(Passenger passenger);
        Task<GeneralReponse> RemoveAsync(int id);
        Task<Passenger?> GetAsync(int id);
        Task<IEnumerable<Passenger>> GetAllAsync();
    }
}
