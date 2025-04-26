using BaseEntity.Entities;
using BaseEntity.Responses;
using Microsoft.EntityFrameworkCore;
using ServerLibrary.Data;
using ServerLibrary.Repositories.Interfaces;
using System.Data.Common;
using System.Linq.Expressions;

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

        public async Task<GeneralReponse> UpdateAsync(Notification notification)
        {
            var existingNotification = await db.Notifications.FirstOrDefaultAsync(x => x.Id == notification.Id);
            if (existingNotification == null) return new GeneralReponse(false, "Notification was not found.");

            try
            {
                if (!string.IsNullOrEmpty(notification.Subject))
                    existingNotification.Subject = notification.Subject;

                if(!string.IsNullOrEmpty(notification.Body))
                    existingNotification.Body = notification.Body;
                if(notification.IsRead != existingNotification.IsRead)
                    existingNotification.IsRead = notification.IsRead;

                await db.SaveChangesAsync();
                return new GeneralReponse(true, "Notification updated successfuly");
            }
            catch (DbException dbEx)
            {
                return new GeneralReponse(false, $"Database error: {dbEx.Message}");
            }
            catch (Exception ex)
            {
                return new GeneralReponse(false, $"Something went wrong {ex.Message}");
            }
        }
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
        public async Task<IEnumerable<Notification>> FindAsync(Expression<Func<Notification, bool>> predicate)
        {
            var notifications = await db.Notifications.Where(predicate).ToListAsync();
            return notifications;
        }
    }
}
