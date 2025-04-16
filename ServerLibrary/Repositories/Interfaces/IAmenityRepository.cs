using BaseEntity.Entities;
using BaseEntity.Responses;

namespace ServerLibrary.Repositories.Interfaces
{
    public interface IAmenityRepository
    {
        Task<GeneralReponse> CreateAsync(Amenity amenity);
        Task<GeneralReponse> DeleteAsync(int id);
        Task<Amenity?> GetByIdAsync(int id);
        Task<IEnumerable<Amenity>> GetAllAsync();
    }
}
