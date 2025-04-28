using AutoMapper;
using Azure;
using BaseEntity.Dtos;
using BaseEntity.Entities;
using BaseEntity.Enums;
using BaseEntity.Responses;
using ServerLibrary.AbstractFactory;
using ServerLibrary.Repositories.Interfaces;

namespace ServerLibrary.Observer
{
    public class ObservableDiscountService : IDiscountService
    {
        private readonly IDiscountRepository _discountRepository;
        private readonly IMapper _mapper;
        private readonly INotificationTemplateGenerator _templateGenerator;
        private readonly List<IObserver<Discount>> _observers = new();

        public ObservableDiscountService(IDiscountRepository discountRepository, IMapper mapper,
             IEnumerable<IObserver<Discount>> observers,
             INotificationTemplateGenerator templateGenerator)
        {
            _discountRepository = discountRepository;
            _mapper = mapper;
            _templateGenerator = templateGenerator;
            _observers.AddRange(observers);
        }
        public async Task<GeneralReponse> AddAsync(CreateDiscountDto createDiscount)
        {
            var discount = _mapper.Map<Discount>(createDiscount);
            var response = await _discountRepository.CreateAsync(discount);

            if (response.Flag)
            {
                await NotifyObservers(discount, ChangeType.Created);
            }

            return response;
        }

        public async Task<GeneralReponse> DeleteAsync(int id)
        {
            var discount = await _discountRepository.GetByIdAsync(id);
            var response = await _discountRepository.DeleteAsync(id);
            if (response.Flag && discount != null)
            {
                await NotifyObservers(discount, ChangeType.Removed);
            }
            return response;
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

            var response = await _discountRepository.UpdateAsync(discount);
            var updatedDiscount = await _discountRepository.GetByIdAsync(discountId);
            if(response.Flag && updatedDiscount != null)
            {
                await NotifyObservers(updatedDiscount, ChangeType.Updated);
            }

            return response;
        }
        private async Task NotifyObservers(Discount discount, ChangeType changeType)
        {
            string subject = "";
            string message = "";

            switch (changeType)
            {
                case ChangeType.Created:
                    subject = "New Discount Created!";
                    message = _templateGenerator.GenerateCreatedDiscountTemplate(discount);
                    break;
                case ChangeType.Removed:
                    subject = "Discount Removed";
                    message = _templateGenerator.GenerateRemovedDiscountTemplate(discount);
                    break;
                case ChangeType.Updated:
                    subject = "Discount Updated";
                    message = _templateGenerator.GenerateUpdatedDiscountTemplate(discount);
                    break;
            }

            foreach (var observer in _observers)
            {
                await observer.Notify(discount, subject, message); 
            }
        }
    }
}
