using BaseEntity.Entities;
using BaseEntity.Responses;
using Microsoft.EntityFrameworkCore;
using ServerLibrary.Data;
using ServerLibrary.Repositories.Interfaces;
using System.Data.Common;

namespace ServerLibrary.Repositories.Implementations
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly AppDbContext db;

        public NotificationRepository(AppDbContext db)
        {
            this.db = db;
        }
        public async Task<GeneralReponse> CreateAsync(Notification notification)
        {
            try
            {
                await db.Notifications.AddAsync(notification);
                await db.SaveChangesAsync();

                return new GeneralReponse(true,"Notification added successfuly.");
               
            }
            catch (DbException dbEx)
            {
                return new GeneralReponse(false, $"Database error: {dbEx.Message}");
            }
            catch (Exception ex)
            {
                return new GeneralReponse(false, $"Something went wrong: {ex.Message}");
            }
        }

        public async Task<IEnumerable<Notification>> GetAllAsync()
             => await db.Notifications.AsNoTracking().ToListAsync();

        public async Task<Notification?> GetAsync(int id)
              => await db.Notifications.FirstOrDefaultAsync(x => x.Id == id);

        public async Task<GeneralReponse> RemoveAsync(int id)
        {
            var notification = await db.Notifications.FirstOrDefaultAsync(x => x.Id == id);
            if (notification == null) return new GeneralReponse(false,"Notification was not found.");

            try
            {
                db.Notifications.Remove(notification);
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
            return new GeneralReponse(true, $"Notification {id} was deleted successfuly");
        }
    }
}
