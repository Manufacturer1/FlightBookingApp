using BaseEntity.Entities;
using BaseEntity.Responses;
using Microsoft.EntityFrameworkCore;
using ServerLibrary.Data;
using ServerLibrary.Repositories.Interfaces;
using System.Data.Common;

namespace ServerLibrary.Repositories.Implementations
{
    public class AmenityRepository : IAmenityRepository
    {
        private readonly AppDbContext db;

        public AmenityRepository(AppDbContext db)
        {
            this.db = db;
        }
        public async Task<GeneralReponse> CreateAsync(Amenity amenity)
        {
            if (amenity == null) return new GeneralReponse(false, "Amenity was not found");

            try
            {
                await db.Amenities.AddAsync(amenity);
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
            return new GeneralReponse(true, $"Amenity {amenity.Id} was added successfully");
        }

        public async Task<GeneralReponse> DeleteAsync(int id)
        {
            var amenity = await db.Amenities.FirstOrDefaultAsync(x => x.Id == id);

            if (amenity == null) return new GeneralReponse(false, "The amenity was not found.");

            try
            {
                db.Amenities.Remove(amenity);
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
            return new GeneralReponse(true, $"Amenity {id} was deleted successfuly");
        }

        public async Task<IEnumerable<Amenity>> GetAllAsync()
             => await db.Amenities.AsNoTracking()
            .Include(x => x.FlightAmenities!)
            .ThenInclude(x => x.Flight)
            .ToListAsync();    

        public async Task<Amenity?> GetByIdAsync(int id)
             => await db.Amenities
            .Include(x => x.FlightAmenities!)
            .ThenInclude(x => x.Flight)
            .FirstOrDefaultAsync(x => x.Id == id);



    }
}
