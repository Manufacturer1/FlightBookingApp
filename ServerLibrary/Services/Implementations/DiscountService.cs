using AutoMapper;
using BaseEntity.Dtos;
using BaseEntity.Entities;
using BaseEntity.Responses;
using ServerLibrary.Repositories.Interfaces;
using ServerLibrary.Services.Interfaces;

namespace ServerLibrary.Services.Implementations
{
    public class DiscountService : IDiscountService
    {
        private readonly IDiscountRepository _discountRepository;
        private readonly IMapper _mapper;

        public DiscountService(IDiscountRepository discountRepository,IMapper mapper)
        {
            _discountRepository = discountRepository;
            _mapper = mapper;
        }
        public async Task<GeneralReponse> AddAsync(CreateDiscountDto createDiscount)
        {
            var discount = _mapper.Map<Discount>(createDiscount);

            return await _discountRepository.CreateAsync(discount);
        }

        public async Task<GeneralReponse> DeleteAsync(int id)
        {
            return await _discountRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<GetDiscountDto>> GetAllAsync()
        {
            var discounts = await _discountRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<GetDiscountDto>>(discounts);
        }

        public async Task<GetDiscountDto?> GetByIdAsync(int id)
        {
           var discount = await _discountRepository.GetByIdAsync(id);
            return _mapper.Map<GetDiscountDto>(discount);
        }

        public async Task<GeneralReponse> UpdateAsync(UpdateDiscountDto updateDiscount, int discountId)
        {
            var discount = _mapper.Map<Discount>(updateDiscount);
            discount.Id = discountId;

            return await _discountRepository.UpdateAsync(discount);
        }
    }
}
