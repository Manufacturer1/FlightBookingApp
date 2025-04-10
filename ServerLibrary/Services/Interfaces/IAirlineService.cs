using BaseEntity.Dtos;
using BaseEntity.Responses;

namespace ServerLibrary.Services.Interfaces
{
    public interface IAirlineService
    {
        Task<GeneralReponse> AddAsync(CreateAirlineDto createAirline);
        Task<GeneralReponse> UpdateAsync(UpdateAirlineDto updateAirline);
        Task<GeneralReponse> DeleteAsync(int id);
        Task<GetAirlineDto?> GetByIdAsync(int id);
        Task<IEnumerable<GetAirlineDto>> GetAllAsync();
    }
}
