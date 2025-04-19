using BaseEntity.Entities;
using BaseEntity.Responses;
using Microsoft.EntityFrameworkCore;
using ServerLibrary.Data;
using ServerLibrary.Repositories.Interfaces;
using System.Data.Common;

namespace ServerLibrary.Repositories.Implementations
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly AppDbContext db;

        public DiscountRepository(AppDbContext db)
        {
            this.db = db;
        }

        public async Task<GeneralReponse> CreateAsync(Discount discount)
        {
           if (discount == null) return new GeneralReponse(false,"Discount is null");

            try
            {
                await db.Discounts.AddAsync(discount);
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
            return new GeneralReponse(true, $"Discount {discount.Id} was added successfully");
        }

        public async Task<GeneralReponse> DeleteAsync(int id)
        {
            var discount = await db.Discounts.FirstOrDefaultAsync(x => x.Id == id);

            if (discount == null) return new GeneralReponse(false, $"Discount {id} was not found.");

            try
            {
                db.Discounts.Remove(discount);
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
            return new GeneralReponse(true, $"Discount {discount.Id} was removed successfully");
        }

        public async Task<IEnumerable<Discount>> GetAllAsync()
            => await db.Discounts.AsNoTracking().ToListAsync();

        public async Task<Discount?> GetByIdAsync(int id)
            => await db.Discounts.FirstOrDefaultAsync(x => x.Id == id);

        public async Task<GeneralReponse> UpdateAsync(Discount discount)
        {
            var entity = await db.Discounts.FirstOrDefaultAsync(x => x.Id  == discount.Id);
            if (entity == null) return new GeneralReponse(false, "Discount was not found.");

            try
            {

            if (!string.IsNullOrEmpty(discount.Name))
                entity.Name = discount.Name;

            if(discount.Percentage != 0m)
                entity.Percentage = discount.Percentage;

            if(discount.IsActive != entity.IsActive)
                entity.IsActive = discount.IsActive;


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
            return new GeneralReponse(true, $"Discount {discount.Id} updated successfully");
        }
    }
}
