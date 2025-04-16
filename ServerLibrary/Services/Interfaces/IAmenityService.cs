using BaseEntity.Dtos;
using BaseEntity.Responses;

namespace ServerLibrary.Services.Interfaces
{
    public interface IAmenityService
    {
        Task<GeneralReponse> AddAsync(CreateAmenityDto createAmenity);
        Task<GeneralReponse> DeleteAsync(int id);
        Task<GetAmenityDto?> GetByIdAsync(int id);
        Task<IEnumerable<GetAmenityDto>> GetAllAsync();
    }
}
