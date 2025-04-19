using BaseEntity.Entities;
using BaseEntity.Responses;

namespace ServerLibrary.Repositories.Interfaces
{
    public interface IDiscountRepository
    {
        Task<GeneralReponse> CreateAsync(Discount discount);
        Task<GeneralReponse> UpdateAsync(Discount discount);
        Task<GeneralReponse> DeleteAsync(int id);
        Task<Discount?> GetByIdAsync(int id);
        Task<IEnumerable<Discount>> GetAllAsync();
    }
}
