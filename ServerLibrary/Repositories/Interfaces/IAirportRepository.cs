using BaseEntity.Entities;
using BaseEntity.Responses;

namespace ServerLibrary.Repositories.Interfaces
{
    public interface IAirportRepository
    {
        Task<GeneralReponse> CreateAsync(Airport airport);
        Task<GeneralReponse> UpdateAsync(Airport airport);
        Task<GeneralReponse> DeleteAsync(int id);
        Task<Airport?> GetByIdAsync(int id);
        Task<IEnumerable<Airport>> GetAllAsync();
    }
}
