using BaseEntity.Dtos;
using BaseEntity.Responses;

namespace ServerLibrary.Services.Interfaces
{
    public interface IItineraryService
    {
        Task<GeneralReponse> AddAsync(CreateItineraryDto itineraryDto);
        Task<IEnumerable<GetItineraryDto>> GetAllAsync();

    }
}
