using BaseEntity.Entities;
using BaseEntity.Responses;
using Microsoft.EntityFrameworkCore;
using ServerLibrary.Data;
using ServerLibrary.Repositories.Interfaces;
using System.Data.Common;

namespace ServerLibrary.Repositories.Implementations
{
    public class PassportIdentityRepository : IPassportIdentityRepository
    {
        private readonly AppDbContext db;

        public PassportIdentityRepository(AppDbContext db)
        {
            this.db = db;
        }
        public async Task<int> CreateAsync(PassportIdentity passportIdentity)
        {

            try
            {
                await db.PassportIdentities.AddAsync(passportIdentity);
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
            return passportIdentity.Id;
        }

        public async Task<IEnumerable<PassportIdentity>> GetAllAsync()
            => await db.PassportIdentities.AsNoTracking().ToListAsync();

        public async Task<PassportIdentity?> GetByIdAsync(int id)
               => await db.PassportIdentities.FirstOrDefaultAsync(x => x.Id == id);

        public async Task<GeneralReponse> RemoveAsync(int id)
        {
            var passport = await db.PassportIdentities.FirstOrDefaultAsync(x => x.Id == id);
            if (passport == null) return new GeneralReponse(false, "Passport does not exist");

            try
            {
                db.PassportIdentities.Remove(passport);
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
            return new GeneralReponse(true, $"Passport {id} was deleted successfuly");
        }

        public async Task<GeneralReponse> UpdateAsync(PassportIdentity passportIdentity)
        {
            var passport = await db.PassportIdentities.FirstOrDefaultAsync(x => x.Id == passportIdentity.Id);
            if (passport == null) return new GeneralReponse(false, "Passport was not found.");

            try
            {
                if(!string.IsNullOrEmpty(passportIdentity.Country))
                    passport.Country = passportIdentity.Country;
                if (!string.IsNullOrEmpty(passportIdentity.PassportNumber))
                    passport.PassportNumber = passportIdentity.PassportNumber;
                if (passportIdentity.ExpiryDate.Date != passport.ExpiryDate.Date)
                    passport.ExpiryDate = passportIdentity.ExpiryDate;

                await db.SaveChangesAsync();

                return new GeneralReponse(true, "Passport updated successfuly.");
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
    }
}
