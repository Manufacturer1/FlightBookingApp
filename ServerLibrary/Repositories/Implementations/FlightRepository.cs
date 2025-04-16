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
        private readonly AppDbContext _db;

        public FlightRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<GeneralReponse> CreateAsync(Flight flight)
        {
            if (flight == null)
                return new GeneralReponse(false, "Flight data cannot be null");

            try
            {
                flight.FlightNumber = Guid.NewGuid().ToString().Substring(0, 6);
                await _db.Flights.AddAsync(flight);
                await _db.SaveChangesAsync();

                return new GeneralReponse(true, $"Flight {flight.FlightNumber} created successfully");
            }
            catch (DbUpdateException dbEx)
            {
                return new GeneralReponse(false, $"Database error: {dbEx.InnerException?.Message ?? dbEx.Message}");
            }
            catch (Exception ex)
            {
                return new GeneralReponse(false, $"Error creating flight: {ex.Message}");
            }
        }

        public async Task<GeneralReponse> DeleteAsync(string flightNumber)
        {
            try
            {
                var flight = await _db.Flights.FirstOrDefaultAsync(x => x.FlightNumber == flightNumber);
                if (flight == null)
                    return new GeneralReponse(false, "Flight not found");

                _db.Flights.Remove(flight);
                await _db.SaveChangesAsync();

                return new GeneralReponse(true, "Flight deleted successfully");
            }
            catch (DbException dbEx)
            {
                return new GeneralReponse(false, $"Database error: {dbEx.Message}");
            }
            catch (Exception ex)
            {
                return new GeneralReponse(false, $"Error deleting flight: {ex.Message}");
            }
        }

        public async Task<IEnumerable<Flight>> GetAllAsync()
              => await _db.Flights.AsNoTracking()
            .Include(x => x.Plane)
            .Include(x => x.Segments)
            .Include(x => x.DestinationAirport)
            .Include(x => x.OriginAirport)
            .ToListAsync();

        public async Task<Flight?> GetByFlightNumberAsync(string flightNumber)
            => await _db.Flights
            .Include(x => x.Plane)
            .Include(x => x.Segments)
            .Include(x => x.DestinationAirport)
            .Include(x => x.OriginAirport)
            .FirstOrDefaultAsync(x => x.FlightNumber == flightNumber);

        public async Task<GeneralReponse> UpdateAsync(Flight flight)
        {
            try
            {
                var existingFlight = await _db.Flights.FirstOrDefaultAsync(x => x.FlightNumber == flight.FlightNumber);
                if (existingFlight == null)
                    return new GeneralReponse(false, "Flight not found");

                // Update properties
                existingFlight.ArrivalDate = flight.ArrivalDate;
                existingFlight.ArrivalTime = flight.ArrivalTime;
                existingFlight.Origin = flight.Origin;
                existingFlight.BasePrice = flight.BasePrice;
                existingFlight.DepartureTime = flight.DepartureTime;
                existingFlight.AvailableSeats = flight.AvailableSeats;
                existingFlight.TotalSeats = flight.TotalSeats;
                existingFlight.ClassType = flight.ClassType;
                existingFlight.DepartureDate = flight.DepartureDate;
                existingFlight.Destination = flight.Destination;
                existingFlight.DestinationImageUrl = flight.DestinationImageUrl;
                existingFlight.TimeIcon = flight.TimeIcon;

                await _db.SaveChangesAsync();

                return new GeneralReponse(true, "Flight updated successfully");
            }
            catch (DbUpdateException dbEx)
            {
                return new GeneralReponse(false, $"Database error: {dbEx.InnerException?.Message ?? dbEx.Message}");
            }
            catch (Exception ex)
            {
                return new GeneralReponse(false, $"Error updating flight: {ex.Message}");
            }
        }
    }
}