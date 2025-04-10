using BaseEntity.Dtos;
using BaseEntity.Responses;

namespace ServerLibrary.Services.Interfaces
{
    public interface IPlaneService
    {
        Task<GeneralReponse> AddAsync(CreatePlaneDto createPlane);
        Task<GeneralReponse> UpdateAsync(CreatePlaneDto updateAirline,int planeId);
        Task<GeneralReponse> DeleteAsync(int id);
        Task<GetPlaneDto?> GetByIdAsync(int id);
        Task<IEnumerable<GetPlaneDto>> GetAllAsync();
    }
}
