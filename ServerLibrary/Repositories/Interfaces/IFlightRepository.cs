using BaseEntity.Entities;
using BaseEntity.Responses;


namespace ServerLibrary.Repositories.Interfaces
{
    public interface IFlightRepository 
    {
        Task<GeneralReponse> CreateAsync(Flight flight);
        Task<GeneralReponse> UpdateAsync(Flight flight);
        Task<GeneralReponse> DeleteAsync(string flightNumber);
        Task<Flight?> GetByFlightNumberAsync(string flightNumber);
        Task<IEnumerable<Flight>> GetAllAsync();

    }
}
