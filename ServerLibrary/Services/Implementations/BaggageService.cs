using AutoMapper;
using BaseEntity.Dtos;
using BaseEntity.Entities;
using BaseEntity.Responses;
using ServerLibrary.Repositories.Interfaces;
using ServerLibrary.Services.Interfaces;

namespace ServerLibrary.Services.Implementations
{
    public class BaggageService : IBaggageService
    {
        private readonly IBaggageRepository _baggageRepository;
        private readonly IMapper _mapper;

        public BaggageService(IBaggageRepository baggageRepository,IMapper mapper)
        {
            _baggageRepository = baggageRepository;
            _mapper = mapper;
        }


        public async Task<GeneralReponse> AddAsync(CreateBaggageDto createBaggage)
        {
            var baggage = _mapper.Map<BaggagePolicy>(createBaggage);

            return await _baggageRepository.CreateAsync(baggage);
        }

        public async Task<GeneralReponse> DeleteAsync(int id)
        {
           return await _baggageRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<GetBaggageDto>> GetAllAsync()
        {
            var baggages = await _baggageRepository.GetAllAsync();

            return _mapper.Map<IEnumerable<GetBaggageDto>>(baggages);   
        }

        public async Task<GetBaggageDto?> GetByIdAsync(int id)
        {
            var baggage = await _baggageRepository.GetByIdAsync(id);

            return _mapper.Map<GetBaggageDto?>(baggage);
        }

        public async Task<GeneralReponse> UpdateAsync(CreateBaggageDto updateBaggage, int baggageId)
        {
            var baggage = _mapper.Map<BaggagePolicy>(updateBaggage);

            baggage.Id = baggageId; 

            return await _baggageRepository.UpdateAsync(baggage);   
        }
    }
}
