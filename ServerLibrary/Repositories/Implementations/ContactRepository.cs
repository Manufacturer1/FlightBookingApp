using BaseEntity.Entities;
using BaseEntity.Responses;
using Microsoft.EntityFrameworkCore;
using ServerLibrary.Data;
using ServerLibrary.Repositories.Interfaces;
using System.Data.Common;
using System.Linq.Expressions;

namespace ServerLibrary.Repositories.Implementations
{
    public class ContactRepository : IContactRepository
    {
        private readonly AppDbContext db;

        public ContactRepository(AppDbContext db)
        {
            this.db = db;
        }
        public async Task<int> CreateAsync(ContactDetails contactDetails)
        {
            try
            {
                await db.ContactDetails.AddAsync(contactDetails);
                await db.SaveChangesAsync();
            }
            catch (DbException dbEx)
            {
                throw new Exception(dbEx.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return contactDetails.Id;
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
        public async Task<GeneralReponse> UpdateAsync(ContactDetails contactDetails)
        {
            var existingContact = await db.ContactDetails.FirstOrDefaultAsync(x => x.Id == contactDetails.Id);
            if (existingContact == null) return new GeneralReponse(false, "Contact was not found.");

            try
            {
                if(!string.IsNullOrEmpty(contactDetails.Surname))
                    existingContact!.Surname = contactDetails.Surname;

                if(!string.IsNullOrEmpty(contactDetails.Name))
                    existingContact!.Name = contactDetails.Name;

                if(!string.IsNullOrEmpty(contactDetails.PhoneNumber))
                    existingContact!.PhoneNumber = contactDetails.PhoneNumber;


                await db.SaveChangesAsync();

                return new GeneralReponse(true, "Contact details updated successfully.");
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
        public async Task<IEnumerable<ContactDetails>> FindAsync(Expression<Func<ContactDetails,bool>> predicate)
        {
            return await db.ContactDetails.Where(predicate).ToListAsync();
        }
    }
}
