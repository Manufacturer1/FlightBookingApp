using BaseEntity.Entities;
using BaseEntity.Responses;

namespace ServerLibrary.Repositories.Interfaces
{
    public interface IFlightAmenityRepository
    {
        Task<GeneralReponse> CreateAsync(FlightAmenity amenity);
        Task<GeneralReponse> UpdateAsync(FlightAmenity amenity);
        Task SaveChangesAsync();
    }
}
