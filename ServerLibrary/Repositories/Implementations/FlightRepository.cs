using BaseEntity.Entities;
using BaseEntity.Responses;
using Microsoft.EntityFrameworkCore;
using ServerLibrary.Data;
using ServerLibrary.Repositories.Interfaces;
using System.Data.Common;

namespace ServerLibrary.Repositories.Implementations
{
    public class FlightRepository : IFlightRepository
    {

        private readonly AppDbContext db;

        public FlightRepository(AppDbContext db)
        {
            this.db = db;
        }

        public async Task<GeneralReponse> CreateAsync(Flight flight)
        {
            if(flight == null) return new GeneralReponse(false,"Flight was not found");

            try
            {
                flight.FlightNumber = Guid.NewGuid().ToString().Substring(0, 6);
                await db.Flights.AddAsync(flight);
                await db.SaveChangesAsync();
            }
            catch(DbException dbEx)
            {
                return new GeneralReponse(false, $"Database error: {dbEx.Message}");
            }
            catch (Exception ex)
            {
                return new GeneralReponse(false ,$"Something went wrong {ex.Message}");
            }
            return new GeneralReponse(true,$"Flight {flight.FlightNumber} added successfully");

        }

        public async Task<GeneralReponse> DeleteAsync(string flightNumber)
        {
            var flight = await db.Flights.FirstOrDefaultAsync(x => x.FlightNumber == flightNumber);

            if (flight == null) return new GeneralReponse(false, "Not found");

            try
            {
                db.Flights.Remove(flight);
                await db.SaveChangesAsync();
            }
            catch(DbException dbEx)
            {
                return new GeneralReponse(false,$"Database error: {dbEx.Message}");
            }
            catch(Exception ex)
            {
                return new GeneralReponse(false, $"Something went wrong: {ex.Message}");
            }
            return new GeneralReponse(true,$"Flight {flightNumber} deleted successfuly");
        }

        public async Task<IEnumerable<Flight>> GetAllAsync()
               => await db.Flights.AsNoTracking().ToListAsync();

        public async Task<Flight?> GetByFlightNumberAsync(string flightNumber)
               => await db.Flights.FirstOrDefaultAsync(x => x.FlightNumber == flightNumber);

        public async Task<GeneralReponse> UpdateAsync(Flight flight)
        {
            var entity = await db.Flights.FirstOrDefaultAsync(x => x.FlightNumber == flight.FlightNumber);

            if (entity == null) return new GeneralReponse(false, "Not found");

            entity.ArrivalDate = flight.ArrivalDate;
            entity.ArrivalTime = flight.ArrivalTime;
            entity.Origin  = flight.Origin; 
            entity.DepartureTime = flight.DepartureTime;
            entity.AvailableSeats = flight.AvailableSeats;
            entity.TotalSeats  = flight.TotalSeats;
            entity.BasePrice = flight.BasePrice;
            entity.ClassType = flight.ClassType;
            entity.DepartureDate = flight.DepartureDate;
            entity.Destination = flight.Destination;
            entity.DestinationImageUrl = flight.DestinationImageUrl;
            entity.TimeIcon = flight.TimeIcon; 
            
            try
            {
                await db.SaveChangesAsync();
            }
            catch(DbException dbEx) {
                return new GeneralReponse(false, $"Database error: {dbEx.Message}");
            }
            return new GeneralReponse(true,$"Flight {flight.FlightNumber} updated successfuly");
        }
    }
}
