using BaseEntity.Entities;
using BaseEntity.Responses;
using Microsoft.EntityFrameworkCore;
using ServerLibrary.Data;
using ServerLibrary.Repositories.Interfaces;
using System.Data.Common;
using System.Linq.Expressions;

namespace ServerLibrary.Repositories.Implementations
{
    public class PassengerRepository : IPassengerRepository
    {
        private readonly AppDbContext db;

        public PassengerRepository(AppDbContext db)
        {
            this.db = db;
        }
        public async Task<int> CreateAsync(Passenger passenger)
        {
            try
            {
                await db.Passengers.AddAsync(passenger);
                await db.SaveChangesAsync();

            }
            catch (DbException dbEx)
            {
                throw new Exception(dbEx.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return passenger.Id;
        }

        public async Task<IEnumerable<Passenger>> GetAllAsync()
               => await db.Passengers.AsNoTracking()
            .Include(x => x.PassportIdentity)
            .Include(x => x.ContactDetails)
            .ToListAsync();

        public async Task<Passenger?> GetAsync(int id)
            => await db.Passengers
            .Include(x => x.PassportIdentity)
            .Include (x => x.ContactDetails)
            .FirstOrDefaultAsync(x => x.Id == id);

        public async Task<GeneralReponse> RemoveAsync(int id)
        {
            var passenger = await db.Passengers.FirstOrDefaultAsync(x => x.Id == id);
            if (passenger == null) return new GeneralReponse(false, "Passenger does not exist.");
            try
            {
                db.Passengers.Remove(passenger);
                await db.SaveChangesAsync();

            }
            catch (DbException dbEx)
            {
                return new GeneralReponse(false, $"Database error: {dbEx.Message}");
            }
            catch (Exception ex)
            {
                return new GeneralReponse(false, $"Something went wrong: {ex.Message}");
            }

            return new GeneralReponse(true, $"Passenger {passenger.Id} was removed successfully");
        }
    }
}
