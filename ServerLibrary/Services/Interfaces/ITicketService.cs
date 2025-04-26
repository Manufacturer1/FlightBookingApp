using BaseEntity.Dtos;
using BaseEntity.Responses;

namespace ServerLibrary.Services.Interfaces
{
    public interface ITicketService
    {
        Task<(GeneralReponse Response, List<GetTicketDto>? tickets)> GenerateTicketAsync(CreateTicketDto createTicket);
        Task<GeneralReponse> RemoveTicketAsync(int id);
        Task<GetTicketDto> GetTicketAsync(int id);
        Task<IEnumerable<GetTicketDto>?> GetTicketByPaymentIntentIdAsync(string paymentIntentId);
    }
}
