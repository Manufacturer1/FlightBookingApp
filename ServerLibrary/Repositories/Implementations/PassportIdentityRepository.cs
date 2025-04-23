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
        public async Task<GeneralReponse> CreateAsync(PassportIdentity passportIdentity)
        {
            if (passportIdentity == null) return new GeneralReponse(false, "Passport is null.");
            try
            {
                await db.PassportIdentities.AddAsync(passportIdentity);
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
            return new GeneralReponse(true, $"Passport {passportIdentity.Id} was added successfully");
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
    }
}
