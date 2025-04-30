using BaseEntity.Dtos;
using BaseEntity.Responses;
using ServerLibrary.Memento;

namespace ServerLibrary.Facade
{
    public interface IBookingFacade
    {
        Task<BookingResponse> BookSeatAsync(CreateBookingDto createBooking, BookingDraftMemento draft);
    }
}
