using BaseEntity.Dtos;
using BaseEntity.Responses;
using ServerLibrary.Memento;

namespace ServerLibrary.Services.Interfaces
{
    public interface IBookingService
    {
        Task<GeneralReponse> BookSeatAsync(CreateBookingDto createBooking);
        Task<GeneralReponse> RemoveBookingAsync(int bookingId);
        Task<GetBookingDto?> GetBookingAsync(int bookingId);
        Task<IEnumerable<GetBookingDto>> GetAllBookingsAsync();
        void SaveBookingState(BookingDraft booking);
        BookingDraftMemento? GetCurrentBookingState();
        void ClearBookingHistory();
    }
}
