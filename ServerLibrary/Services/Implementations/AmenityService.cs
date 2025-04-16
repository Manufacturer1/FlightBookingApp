using AutoMapper;
using BaseEntity.Dtos;
using BaseEntity.Entities;
using BaseEntity.Responses;
using ServerLibrary.Repositories.Interfaces;
using ServerLibrary.Services.Interfaces;

namespace ServerLibrary.Services.Implementations
{
    public class AmenityService : IAmenityService
    {
        private readonly IAmenityRepository _amenityRepository;
        private readonly IMapper _mapper;
        private readonly IFlightRepository _flightRepository;

        public AmenityService(IAmenityRepository amenityRepository,IMapper mapper,IFlightRepository flightRepository)
        {
            _amenityRepository = amenityRepository;
            _mapper = mapper;
            _flightRepository = flightRepository;
        }

        public async Task<GeneralReponse> AddAsync(CreateAmenityDto createAmenity)
        {
            if (createAmenity == null)
                return new GeneralReponse(false, "Amenity data is required");

            if (createAmenity.FlightAmenities == null || !createAmenity.FlightAmenities.Any())
                return new GeneralReponse(false, "At least one flight amenity is required");

            var flights = new List<Flight>();
            foreach (var amenity in createAmenity.FlightAmenities)
            {
                var flight = await _flightRepository.GetByFlightNumberAsync(amenity.FlightNumber);
                if (flight == null)
                    return new GeneralReponse(false, $"Flight {amenity.FlightNumber} was not found.");
                flights.Add(flight);
            }

            var amenityEntity = _mapper.Map<Amenity>(createAmenity);

            amenityEntity.FlightAmenities = flights.Select(flight => new FlightAmenity
            {
                FlightNumber = flight.FlightNumber
            }).ToList();

            try
            {
               
                return await _amenityRepository.CreateAsync(amenityEntity);
            }
            catch (Exception ex)
            {
                return new GeneralReponse(false, $"Something went wrong: {ex.Message}");
            }
        }


        public async Task<GeneralReponse> DeleteAsync(int id)
        {
            return await _amenityRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<GetAmenityDto>> GetAllAsync()
        {
            var amenities = await _amenityRepository.GetAllAsync();

            return _mapper.Map<IEnumerable<GetAmenityDto>>(amenities);
        }

        public async Task<GetAmenityDto?> GetByIdAsync(int id)
        {
            var amenity = await _amenityRepository.GetByIdAsync(id);

            return _mapper.Map<GetAmenityDto>(amenity);
        }

    }
}
