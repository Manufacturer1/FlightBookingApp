using BaseEntity.Entities;
using BaseEntity.Responses;
using Microsoft.EntityFrameworkCore;
using ServerLibrary.Data;
using ServerLibrary.Repositories.Interfaces;
using System.Data.Common;

namespace ServerLibrary.Repositories.Implementations
{
    public class ItineraryRepository : IItineraryRepository
    {
        private readonly AppDbContext db;

        public ItineraryRepository(AppDbContext db)
        {
            this.db = db;
        }
        public async Task<GeneralReponse> AddAsync(Itinerary itinerary)
        {
            if (itinerary == null) return new GeneralReponse(false, "Itinerary is null");

            try
            {
                await db.Itineraries.AddAsync(itinerary);
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException dbEx)
            {
                return new GeneralReponse(false, $"Database error: {dbEx.Message}");
            }
            catch (Exception ex)
            {
                return new GeneralReponse(false, $"Error creating flight: {ex.Message}");
            }
            return new GeneralReponse(true, $"Itinerary {itinerary.Id} created successfully");
        }

        public async Task<GeneralReponse> DeleteAsync(int id)
        {
            var itinerary = await db.Itineraries.FirstOrDefaultAsync(x => x.Id == id);
            if (itinerary == null) return new GeneralReponse(false, "Itinerary was not found.");

            try
            {
                db.Itineraries.Remove(itinerary);
            }
            catch (DbException dbEx)
            {
                return new GeneralReponse(false, $"Database error: {dbEx.Message}");
            }
            catch (Exception ex)
            {
                return new GeneralReponse(false, $"Something went wrong {ex.Message}");
            }
            return new GeneralReponse(true, $"Itinerary {id} was removed successfully");

        }

        public async Task<IEnumerable<Itinerary>> GetAllAsync()
               => await db.Itineraries
                            .AsNoTracking()
                            .Include(x => x.Airline)
                            .Include(x => x.Segments!)
                            .ThenInclude(x => x.Flight)
                            .ToListAsync(); 

        public async Task<Itinerary?> GetByIdAsync(int id)
               => await db.Itineraries
                            .Include(x => x.Airline)
                           .Include(x => x.Segments!)
                            .ThenInclude(x => x.Flight)
                            .FirstOrDefaultAsync(x => x.Id == id);

        public async Task<GeneralReponse> UpdateAsync(Itinerary itinerary)
        {
            try
            {
                db.Itineraries.Update(itinerary);
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
            return new GeneralReponse(true, $"Itinerary {itinerary.Id} was updated successfully");
        }
    }
}
