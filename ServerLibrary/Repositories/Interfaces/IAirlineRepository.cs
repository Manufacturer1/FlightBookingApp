using BaseEntity.Entities;
using BaseEntity.Responses;

namespace ServerLibrary.Repositories.Interfaces
{
    public interface IAirlineRepository
    {
        Task<GeneralReponse> CreateAsync(Airline airline);
        Task<GeneralReponse> UpdateAsync(Airline airline);
        Task<GeneralReponse> DeleteAsync(int id);
        Task<Airline?> GetByIdAsync(int id);
        Task<IEnumerable<Airline>> GetAllAsync();
    }
}
