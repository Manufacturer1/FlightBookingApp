using BaseEntity.Dtos;
using BaseEntity.Entities;
using BaseEntity.Responses;

namespace ServerLibrary.Observer
{
    public interface IDiscountService
    {
        Task<GeneralReponse> AddAsync(CreateDiscountDto createDiscount);
        Task<GeneralReponse> UpdateAsync(UpdateDiscountDto updateDiscount, int discountId);
        Task<GeneralReponse> DeleteAsync(int id);
        Task<GetDiscountDto?> GetByIdAsync(int id);
        Task<IEnumerable<GetDiscountDto>> GetAllAsync();
    }
}
