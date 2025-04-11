using BaseEntity.Entities;
using BaseEntity.Responses;
using Microsoft.EntityFrameworkCore;
using ServerLibrary.Data;
using ServerLibrary.Repositories.Interfaces;
using System.Data.Common;

namespace ServerLibrary.Repositories.Implementations
{
    public class PlaneRepository : IPlaneRepository
    {
        private readonly AppDbContext db;

        public PlaneRepository(AppDbContext db)
        {
            this.db = db;
        }

        public async Task<GeneralReponse> CreateAsync(Plane plane)
        {
            if (plane == null) return new GeneralReponse(false, "Plane was not found");

            try
            {
                await db.Planes.AddAsync(plane);
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

            return new GeneralReponse(true,$"Plane {plane.Id} was added successfully");
        }

        public async Task<GeneralReponse> DeleteAsync(int id)
        {
            var plane = await db.Planes.FirstOrDefaultAsync(x => x.Id == id);
            if(plane == null) return new GeneralReponse(false,"Plane was not found");

            try
            {
                db.Remove(plane);
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
            return new GeneralReponse(true,"Plane was removed successfully");
        }

        public async Task<IEnumerable<Plane>> GetAllAsync()
              => await db.Planes
            .AsNoTracking()
            .Include(x => x.Flights)
            .ToListAsync();

        public async Task<Plane?> GetByIdAsync(int id)
                => await db.Planes
                         .Include(x => x.Flights)
                          .FirstOrDefaultAsync(x => x.Id == id);

        public async Task<GeneralReponse> UpdateAsync(Plane plane)
        {
            var entity = await db.Planes.FirstOrDefaultAsync(x => x.Id == plane.Id);
            if (entity == null) return new GeneralReponse(false, "Plane was not found");

            entity.Model = plane.Model;
            entity.Name = plane.Name;
            entity.SeatPitch = plane.SeatPitch;
            entity.SeatLayout = plane.SeatLayout;

            try
            {
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
            return new GeneralReponse(true, $"Plane {plane.Id} was updated successfully");
        }
    }
}
