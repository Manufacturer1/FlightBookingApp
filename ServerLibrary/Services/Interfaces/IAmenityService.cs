using BaseEntity.Dtos;
using BaseEntity.Responses;

namespace ServerLibrary.Services.Interfaces
{
    public interface IAmenityService
    {
        Task<GeneralReponse> AddAsync(CreateAmenityDto createAmenity);
        Task<GeneralReponse> UpdateAmenityAsync(UpdateAmenityDto updateAmenity, int amenityId);
        Task<GeneralReponse> UpdateFlightAmenitiesAsync(IEnumerable<FlightAmenityDto> updateAmenity, int amenityId);
        Task<GeneralReponse> DeleteAsync(int id);
        Task<GetAmenityDto?> GetByIdAsync(int id);
        Task<IEnumerable<GetAmenityDto>> GetAllAsync();
        Task<GeneralReponse> AddFlightsToAmenities(IEnumerable<FlightAmenityDto> flightAmenitiesDto, int amenityId);
    }
}
