using BaseEntity.Entities;
using BaseEntity.Responses;
using Microsoft.EntityFrameworkCore;
using ServerLibrary.Data;
using ServerLibrary.Repositories.Interfaces;
using System.Data.Common;

namespace ServerLibrary.Repositories.Implementations
{
    public class BookingRepository : IBookingRepository
    {
        private readonly AppDbContext db;

        public BookingRepository(AppDbContext db)
        {
            this.db = db;
        }
        public async Task<(GeneralReponse Response, int? BookingId)> CreateAsync(Booking booking)
        {
            if (booking == null)
                return (new GeneralReponse(false, "Booking is null."), null);

            try
            {
                await db.Bookings.AddAsync(booking);
                await db.SaveChangesAsync();
                return (new GeneralReponse(true, $"Booking {booking.Id} was added successfully"), booking.Id);
            }
            catch (DbException dbEx)
            {
                return (new GeneralReponse(false, $"Database error: {dbEx.Message}"), null);
            }
            catch (Exception ex)
            {
                return (new GeneralReponse(false, $"Something went wrong: {ex.Message}"), null);
            }
        }

        public async Task<IEnumerable<Booking>> GetAllAsync()
            => await db.Bookings.AsNoTracking()
            .Include(x => x.Passenger)
            .Include(x => x.Itinerary)
            .ThenInclude(x => x!.Segments!)
            .ThenInclude(x => x.Flight)
            .ToListAsync();

        public async Task<Booking?> GetAsync(int id)
              => await db.Bookings
            .Include(x => x.Passenger)
             .Include(x => x.Itinerary)
            .ThenInclude(x => x!.Segments!)
            .ThenInclude(x => x.Flight)
            .FirstOrDefaultAsync(x => x.Id == id);

        public async Task<GeneralReponse> RemoveAsync(int id)
        {
            var booking = await db.Bookings.FirstOrDefaultAsync(x => x.Id == id);
            if (booking == null) return new GeneralReponse(false, "Booking does not exist.");

            try
            {
                db.Bookings.Remove(booking);
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

            return new GeneralReponse(true, $"Booking {booking.Id} was removed successfully");
        }


    }
}
