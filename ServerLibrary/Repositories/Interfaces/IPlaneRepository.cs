using BaseEntity.Entities;
using BaseEntity.Responses;

namespace ServerLibrary.Repositories.Interfaces
{
    public interface IPlaneRepository
    {
        Task<GeneralReponse> CreateAsync(Plane plane);
        Task<GeneralReponse> UpdateAsync(Plane plane);
        Task<GeneralReponse> DeleteAsync(int id);
        Task<Plane?> GetByIdAsync(int id);
        Task<IEnumerable<Plane>> GetAllAsync();
    }
}
