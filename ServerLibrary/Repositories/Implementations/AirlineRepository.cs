using BaseEntity.Entities;
using BaseEntity.Responses;
using Microsoft.EntityFrameworkCore;
using ServerLibrary.Data;
using ServerLibrary.Repositories.Interfaces;
using System.Data.Common;

namespace ServerLibrary.Repositories.Implementations
{
    public class AirlineRepository : IAirlineRepository
    {
        private readonly AppDbContext db;
        public AirlineRepository(AppDbContext db)
        {
            this.db = db;
        }
        public async Task<GeneralReponse> CreateAsync(Airline airline)
        {
            if (airline == null) return new GeneralReponse(false, "Airline was not found");
            try
            {
                await db.Airlines.AddAsync(airline);
                await db.SaveChangesAsync();

            }
            catch(DbException dbEx)
            {
                return new GeneralReponse(false, $"Database error: {dbEx.Message}");
            }
            catch (Exception ex)
            {
                return new GeneralReponse(false, $"Something went wrong {ex.Message}");
            }
            return new GeneralReponse(true,$"Airline {airline.Id} was added successfully");
        }

        public async Task<GeneralReponse> DeleteAsync(int id)
        {
            var airline = await db.Airlines.FirstOrDefaultAsync(x => x.Id == id);

            if (airline == null) return new GeneralReponse(false, "The airline was not found");

            try
            {
                db.Airlines.Remove(airline);
                await db.SaveChangesAsync();
            }
            catch(DbException dbEx)
            {
                return new GeneralReponse(false, $"Database error: {dbEx.Message}");
            }
            catch (Exception ex)
            {
                return new GeneralReponse(false, $"Something went wrong {ex.Message}");
            }
            return new GeneralReponse(true, $"Airline {id} was deleted successfuly");
        }

        public async Task<IEnumerable<Airline>> GetAllAsync()
               => await db.Airlines.AsNoTracking()
            .Include(x => x.Flights)
            .Include(x => x.BaggagePolicy)
            .ToListAsync();

        public async Task<Airline?> GetByIdAsync(int id)
            => await db.Airlines
            .Include(x => x.Flights)
            .Include(x => x.BaggagePolicy)
            .FirstOrDefaultAsync(x => x.Id == id);

        public async Task<GeneralReponse> UpdateAsync(Airline airline)
        {
           var entity = await db.Airlines.FirstOrDefaultAsync(x => x.Id == airline.Id);
            if (entity == null) return new GeneralReponse(false, "Airline was not found");

            entity.AirlineImageUrl = airline.AirlineImageUrl;
            entity.Name = airline.Name;
            entity.AirlineBgColor = airline.AirlineBgColor;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbException dbEx)
            {
                return new GeneralReponse(false, $"Database error: {dbEx.Message}");
            }

            return new GeneralReponse(true, $"Airline {airline.Id} was updated successfully");
        }
    }
}
