using BaseEntity.Entities;
using BaseEntity.Responses;
using Microsoft.EntityFrameworkCore;
using ServerLibrary.Data;
using ServerLibrary.Repositories.Interfaces;
using System.Data.Common;

namespace ServerLibrary.Repositories.Implementations
{
    public class BaggageRepository : IBaggageRepository
    {
        private readonly AppDbContext db;

        public BaggageRepository(AppDbContext db)
        {
            this.db = db;
        }

        public async Task<GeneralReponse> CreateAsync(BaggagePolicy baggage)
        {
            if(baggage == null) return new GeneralReponse(false,"Baggage is null");

            try
            {
                await db.Baggages.AddAsync(baggage);
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

            return new GeneralReponse(true,"Baggage was added successfully");
        }

        public async Task<GeneralReponse> DeleteAsync(int id)
        {
            var baggage = await db.Baggages.FirstOrDefaultAsync(x => x.Id == id);   
            if(baggage == null) return new GeneralReponse(false,"Baggage was not found");

            try
            {
                db.Baggages.Remove(baggage);
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

            return new GeneralReponse(true, "Baggage was removed successfully");

        }

        public async Task<IEnumerable<BaggagePolicy>> GetAllAsync()
                => await db.Baggages.AsNoTracking()
                      .Include(x => x.Airlines)
                         .ToListAsync();

        public async Task<BaggagePolicy?> GetByIdAsync(int id)
            => await db.Baggages.Include(x => x.Airlines).FirstOrDefaultAsync(x => x.Id == id);

        public async Task<GeneralReponse> UpdateAsync(BaggagePolicy baggage)
        {
            var entity = await db.Baggages.FirstOrDefaultAsync(x => x.Id == baggage.Id);
            if(entity == null) return new GeneralReponse(false,"Baggage was not found");

            entity.FreeCheckedBags = baggage.FreeCheckedBags;
            entity.CheckedWeightLimitKg = baggage.CabinWeightLimitKg;
            entity.ExtraCheckedBagFee = baggage.ExtraCheckedBagFee;
            entity.OverWeightFeePerKg = baggage.OverWeightFeePerKg;
            entity.FreeCabinBags = baggage.FreeCabinBags;
            entity.CabinWeightLimitKg = baggage.CabinWeightLimitKg;
            entity.ExtraCabinBagFee = baggage.ExtraCabinBagFee;

            try
            {
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

            return new GeneralReponse(true,$"Baggage {baggage.Id} was updated successfully");


        }
    }
}
