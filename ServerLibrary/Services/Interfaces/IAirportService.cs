using BaseEntity.Dtos;
using BaseEntity.Responses;

namespace ServerLibrary.Services.Interfaces
{
    public interface IAirportService
    {
        Task<GeneralReponse> AddAsync(CreateAirportDto createAirport);
        Task<GeneralReponse> UpdateAsync(CreateAirportDto updateAirport,int airportId);
        Task<GeneralReponse> DeleteAsync(int id);
        Task<GetAirportDto?> GetByIdAsync(int id);
        Task<IEnumerable<GetAirportDto>> GetAllAsync();
    }
}
