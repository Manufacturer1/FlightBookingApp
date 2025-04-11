using BaseEntity.Entities;
using BaseEntity.Responses;

namespace ServerLibrary.Repositories.Interfaces
{
    public interface IItineraryRepository
    {
        Task<GeneralReponse> AddAsync(Itinerary itinerary);
        Task<GeneralReponse> UpdateAsync(Itinerary itinerary);
        Task<GeneralReponse> DeleteAsync(int id);
        Task<Itinerary?> GetByIdAsync(int id);
        Task<IEnumerable<Itinerary>> GetAllAsync();
    }
}
