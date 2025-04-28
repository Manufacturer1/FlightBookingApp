using BaseEntity.Dtos;
using BaseEntity.Responses;

namespace ServerLibrary.Facade
{
    public interface IBookingFacade
    {
        Task<BookingResponse> BookSeatAsync(CreateBookingDto request);
    }
}
