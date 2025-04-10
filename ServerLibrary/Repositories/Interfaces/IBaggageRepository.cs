using BaseEntity.Entities;
using BaseEntity.Responses;

namespace ServerLibrary.Repositories.Interfaces
{
    public interface IBaggageRepository
    {
        Task<GeneralReponse> CreateAsync(BaggagePolicy baggage);
        Task<GeneralReponse> UpdateAsync(BaggagePolicy baggage);
        Task<GeneralReponse> DeleteAsync(int id);
        Task<BaggagePolicy?> GetByIdAsync(int id);
        Task<IEnumerable<BaggagePolicy>> GetAllAsync();
    }
}
