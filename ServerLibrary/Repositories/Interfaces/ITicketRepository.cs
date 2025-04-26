using BaseEntity.Entities;
using BaseEntity.Responses;
using System.Linq.Expressions;

namespace ServerLibrary.Repositories.Interfaces
{
    public interface ITicketRepository
    {
        Task<(GeneralReponse Response, int? tiketId)> CreateAsync(Ticket ticket);
        Task<GeneralReponse> DeleteAsync(int id);   
        Task<Ticket?> GetAsync(int id);
        Task<IEnumerable<Ticket>> GetAllAsync();
        Task<IEnumerable<Ticket>> GetByPredicate(Expression<Func<Ticket, bool>> predicate);
    }
}
