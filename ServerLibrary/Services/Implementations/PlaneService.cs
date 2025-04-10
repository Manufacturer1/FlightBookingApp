using AutoMapper;
using BaseEntity.Dtos;
using BaseEntity.Entities;
using BaseEntity.Responses;
using ServerLibrary.Repositories.Interfaces;
using ServerLibrary.Services.Interfaces;

namespace ServerLibrary.Services.Implementations
{
    public class PlaneService : IPlaneService
    {
        private readonly IPlaneRepository _planeRepository;
        private readonly IMapper _mapper;

        public PlaneService(IPlaneRepository planeRepository,IMapper mapper)
        {
            _planeRepository = planeRepository;
            _mapper = mapper;
        }

        public async Task<GeneralReponse> AddAsync(CreatePlaneDto createPlane)
        {
            var plane = _mapper.Map<Plane>(createPlane);

            return await _planeRepository.CreateAsync(plane);
        }

        public async Task<GeneralReponse> DeleteAsync(int id)
        {
            return await _planeRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<GetPlaneDto>> GetAllAsync()
        {
            var planes = await _planeRepository.GetAllAsync();

            return _mapper.Map<IEnumerable<GetPlaneDto>>(planes);
        }

        public async Task<GetPlaneDto?> GetByIdAsync(int id)
        {
            var plane = await _planeRepository.GetByIdAsync(id);
            return _mapper.Map<GetPlaneDto>(plane);
        }

        public async Task<GeneralReponse> UpdateAsync(CreatePlaneDto updateAirline, int planeId)
        {
            var plane = _mapper.Map<Plane>(updateAirline);
            plane.Id = planeId;

            return await _planeRepository.UpdateAsync(plane);
        }
    }
}
