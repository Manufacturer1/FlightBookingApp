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
        private readonly IFlightAmenityRepository _flightAmenityRepository;

        public AmenityService(IAmenityRepository amenityRepository,IMapper mapper,IFlightRepository flightRepository,IFlightAmenityRepository flightAmenityRepository)
        {
            _amenityRepository = amenityRepository;
            _mapper = mapper;
            _flightRepository = flightRepository;
            _flightAmenityRepository = flightAmenityRepository;
        }

        public async Task<GeneralReponse> AddAsync(CreateAmenityDto createAmenity)
        {
            if (createAmenity == null)
                return new GeneralReponse(false, "Amenity data is required");

            try
            {
                var amenityEntity = _mapper.Map<Amenity>(createAmenity);
                return await _amenityRepository.CreateAsync(amenityEntity);
            }
            catch (Exception ex)
            {
                return new GeneralReponse(false, $"Something went wrong: {ex.Message}");
            }
        }


        public async Task<GeneralReponse> AddFlightsToAmenities(IEnumerable<FlightAmenityDto>flightAmenitiesDto,int amenityId)
        {
            if (flightAmenitiesDto == null)
                return new GeneralReponse(false, "Flight Amenity Dto was null");

            var amenity = await _amenityRepository.GetByIdAsync(amenityId);
            if (amenity == null)
                return new GeneralReponse(false, "Amenity was not found");



            foreach(var flight in flightAmenitiesDto)
            {
                var fl = await _flightRepository.GetByFlightNumberAsync(flight.FlightNumber);

                if (fl == null)
                    return new GeneralReponse(false, "Flight was not found");

                var flightAmenity = _mapper.Map<FlightAmenity>(flight);
                

                flightAmenity.AmenityId = amenityId;

                var response = await _flightAmenityRepository.CreateAsync(flightAmenity);

                if (!response.Flag)
                    return new GeneralReponse(false, response.Message);
                

            }
            await _flightAmenityRepository.SaveChangesAsync();
            return new GeneralReponse(true,"Amenities successfuly associated with flights");

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

        public async Task<GeneralReponse> UpdateAmenityAsync(UpdateAmenityDto updateAmenity, int amenityId)
        {
            var amenity = _mapper.Map<Amenity>(updateAmenity);

            amenity.Id = amenityId;

            return await _amenityRepository.UpdateAsync(amenity);
        }

        public async Task<GeneralReponse> UpdateFlightAmenitiesAsync(IEnumerable<FlightAmenityDto> updateAmenity, int amenityId)
        {
            var amenity = await _amenityRepository.GetByIdAsync(amenityId);

            if(amenity == null)
                return new GeneralReponse(false,"The amenity was not found");

            foreach(var  flight in updateAmenity)
            {
                var flightAmenity = _mapper.Map<FlightAmenity>(flight);

                flightAmenity.AmenityId = amenityId;

                var fl = await _flightRepository.GetByFlightNumberAsync(flight.FlightNumber);

                if (fl == null)
                    return new GeneralReponse(false, "Flight was not found");

                var response = await _flightAmenityRepository.UpdateAsync(flightAmenity);

                if(!response.Flag)
                    return new GeneralReponse(false,response.Message);

            }


            await _flightAmenityRepository.SaveChangesAsync();

            return new GeneralReponse(true, "Flight amenities updated successfully");
        }
    }
}
