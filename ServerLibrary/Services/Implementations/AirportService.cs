using AutoMapper;
using BaseEntity.Dtos;
using BaseEntity.Entities;
using BaseEntity.Responses;
using ServerLibrary.Repositories.Interfaces;
using ServerLibrary.Services.Interfaces;

namespace ServerLibrary.Services.Implementations
{
    public class AirportService : IAirportService
    {
        private readonly IAirportRepository _airportRespository;
        private readonly IMapper _mapper;
        public AirportService(IAirportRepository airportRepository,IMapper mapper)
        {
            _airportRespository = airportRepository;
            _mapper = mapper;
        }
        public async Task<GeneralReponse> AddAsync(CreateAirportDto createAirport)
        {
            var airport = _mapper.Map<Airport>(createAirport);
            var result = await _airportRespository.CreateAsync(airport);

            return result;
        }

        public async Task<GeneralReponse> DeleteAsync(int id)
        {
            return await _airportRespository.DeleteAsync(id);
        }

        public async Task<IEnumerable<GetAirportDto>> GetAllAsync()
        {
           var airports = await _airportRespository.GetAllAsync();
            
            return _mapper.Map<IEnumerable<GetAirportDto>>(airports);
        }

        public async Task<GetAirportDto?> GetByIdAsync(int id)
        {
            var airport = await _airportRespository.GetByIdAsync(id);

            return _mapper.Map<GetAirportDto?>(airport);
        }

        public async Task<GeneralReponse> UpdateAsync(CreateAirportDto updateAirport, int airportId)
        {
            var airport = _mapper.Map<Airport>(updateAirport);
            airport.Id = airportId;

            var result = await _airportRespository.UpdateAsync(airport);

            return result;
        }
    }
}
