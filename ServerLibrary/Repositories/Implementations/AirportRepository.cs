using BaseEntity.Entities;
using BaseEntity.Responses;
using Microsoft.EntityFrameworkCore;
using ServerLibrary.Data;
using ServerLibrary.Repositories.Interfaces;
using System.Data.Common;

namespace ServerLibrary.Repositories.Implementations
{
    public class AirportRepository : IAirportRepository
    {

        private readonly AppDbContext db;

        public AirportRepository(AppDbContext db)
        {
            this.db = db;
        }
        public async Task<GeneralReponse> CreateAsync(Airport airport)
        {
            if (airport == null) return new GeneralReponse(false, "Airport was not found");

            try
            {
                await db.Airports.AddAsync(airport);
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
            return new GeneralReponse(true, $"Airport {airport.Id} was added successfully");
        }

        public async Task<GeneralReponse> DeleteAsync(int id)
        {
            var airport = await db.Airports
                .Include(x => x.ArrivalFlights)
                .Include(x => x.DepartingFlights)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (airport == null) return new GeneralReponse(false, "The airport was not found");

            foreach(var flight in airport.DepartingFlights!)
            {
                flight.OriginAirportId = null;
                flight.OriginAirport = null;
            }
            foreach(var flight in airport.ArrivalFlights!)
            {
                flight.DestinationAirportId = null;
                flight.DestinationAirport = null;
            }

            try
            {
                db.Airports.Remove(airport);
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
            return new GeneralReponse(true, $"Airport {id} was deleted successfuly");
        }

        public async Task<IEnumerable<Airport>> GetAllAsync()
                => await db.Airports.AsNoTracking().ToListAsync();

        public async Task<Airport?> GetByIdAsync(int id)
               => await db.Airports.FirstOrDefaultAsync(x => x.Id == id);

        public async Task<GeneralReponse> UpdateAsync(Airport airport)
        {
            if (airport == null)
                return new GeneralReponse(false, "Airport was not found");

            var existingAirport = await db.Airports
                .FirstOrDefaultAsync(x => x.Id == airport.Id);

            if (existingAirport == null)
                return new GeneralReponse(false, "The airport to update was not found");

            try
            {
                db.Entry(existingAirport).CurrentValues.SetValues(airport);

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

            return new GeneralReponse(true, $"Airport {airport.Id} was updated successfully");
        }
    }
}
