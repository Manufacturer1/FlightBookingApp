using BaseEntity.Entities;
using BaseEntity.Responses;
using Microsoft.EntityFrameworkCore;
using ServerLibrary.Data;
using ServerLibrary.Repositories.Interfaces;
using System.Data.Common;
using System.Linq.Expressions;

namespace ServerLibrary.Repositories.Implementations
{
    public class TicketRepository : ITicketRepository
    {

        private readonly AppDbContext db;

        public TicketRepository(AppDbContext db)
        {
            this.db = db;
        }
        public async Task<(GeneralReponse Response,int? tiketId)> CreateAsync(Ticket ticket)
        {
            try
            {
                await db.Tickets.AddAsync(ticket);
                await db.SaveChangesAsync();
            }
            catch (DbException dbEx)
            {
                return (new GeneralReponse(false, $"Database error: {dbEx.Message}"), null);
            }
            catch (Exception ex)
            {
                return (new GeneralReponse(false, $"Something went wrong: {ex.Message}"), null);
            }
            return (new GeneralReponse(true, $"Booking {ticket.Id} was added successfully"), ticket.Id);
        }

        public async Task<GeneralReponse> DeleteAsync(int id)
        {
            var ticket = await db.Tickets.FirstOrDefaultAsync(x => x.Id == id);
            if (ticket == null) return new GeneralReponse(false, "Ticket not found.");

            try
            {
                db.Tickets.Remove(ticket);
                await db.SaveChangesAsync();
            }
            catch (DbException dbEx)
            {
                return new GeneralReponse(false, $"Database error: {dbEx.Message}");
            }
            catch (Exception ex)
            {
                return new GeneralReponse(false, $"Something went wrong {ex.Message}");
            }
            return new GeneralReponse(true, $"Ticket {id} was deleted successfuly");
        }

        public async Task<IEnumerable<Ticket>> GetAllAsync()
           => await db.Tickets.AsNoTracking().ToListAsync();

        public async Task<Ticket?> GetAsync(int id)
            => await db.Tickets.FirstOrDefaultAsync(x => x.Id == id);

        public async Task<IEnumerable<Ticket>> GetByPredicate(Expression<Func<Ticket, bool>> predicate)
        {
            return await db.Tickets.Where(predicate).ToListAsync(); 
        }
    }
}
