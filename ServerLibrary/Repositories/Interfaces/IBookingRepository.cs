using BaseEntity.Entities;
using BaseEntity.Responses;

namespace ServerLibrary.Repositories.Interfaces
{
    public interface IBookingRepository
    {
        Task<(GeneralReponse Response, int? BookingId)> CreateAsync(Booking booking);
        Task<GeneralReponse> RemoveAsync(int id);
        Task<Booking?> GetAsync(int id);
        Task<IEnumerable<Booking>> GetAllAsync();
    }
}
