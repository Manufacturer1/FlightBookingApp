using BaseEntity.Entities;
using BaseEntity.Responses;
using Microsoft.EntityFrameworkCore;
using ServerLibrary.Data;
using ServerLibrary.Repositories.Interfaces;
using System.Data.Common;

namespace ServerLibrary.Repositories.Implementations
{
    public class ContactRepository : IContactRepository
    {
        private readonly AppDbContext db;

        public ContactRepository(AppDbContext db)
        {
            this.db = db;
        }
        public async Task<GeneralReponse> CreateAsync(ContactDetails contactDetails)
        {
            if (contactDetails == null) return new GeneralReponse(false, "Contact details is null.");
            try
            {
                await db.ContactDetails.AddAsync(contactDetails);
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
            return new GeneralReponse(true, $"Airport {contactDetails.Id} was added successfully");
        }

        public async Task<IEnumerable<ContactDetails>> GetAllAsync()
            => await db.ContactDetails.AsNoTracking().ToListAsync();

        public async Task<ContactDetails?> GetAsync(int id)
            => await db.ContactDetails.FirstOrDefaultAsync(x => x.Id == id);

        public async Task<GeneralReponse> RemoveAsync(int id)
        {
            var contactDetails = await db.ContactDetails.FirstOrDefaultAsync(x => x.Id == id);
            if (contactDetails == null) return new GeneralReponse(false, "Contact does not exist.");

            try
            {
                db.ContactDetails.Remove(contactDetails);
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
            return new GeneralReponse(true, $"Contact {id} was deleted successfuly");
        }
    }
}
