using BaseEntity.Dtos;
using BaseEntity.Responses;

namespace ServerLibrary.Services.Interfaces
{
    public interface IBaggageService
    {
        Task<GeneralReponse> AddAsync(CreateBaggageDto createBaggage);
        Task<GeneralReponse> UpdateAsync(CreateBaggageDto updateBaggage,int baggageId);
        Task<GeneralReponse> DeleteAsync(int id);
        Task<GetBaggageDto?> GetByIdAsync(int id);
        Task<IEnumerable<GetBaggageDto>> GetAllAsync();
    }
}
