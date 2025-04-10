using AutoMapper;
using BaseEntity.Dtos;
using BaseEntity.Entities;
using BaseEntity.Responses;
using Microsoft.AspNetCore.Http;
using ServerLibrary.Repositories.Interfaces;
using ServerLibrary.Services.Interfaces;

namespace ServerLibrary.Services.Implementations
{
    public class AirlineService : IAirlineService
    {
        private IAirlineRepository _airlineRepository;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;

        public AirlineService(IAirlineRepository airlineRepository,IMapper mapper,IFileService fileService)
        {
            _airlineRepository = airlineRepository;   
            _mapper = mapper;
            _fileService = fileService;
        }

        public async Task<GeneralReponse> AddAsync(CreateAirlineDto createAirline)
        {
            var airline = _mapper.Map<Airline>(createAirline);

            try
            {
                if (createAirline.AirlineImage?.Length > 1 * 1024 * 1024)
                    return new GeneralReponse(false, "File size should not exceed 1 MB");

                var fileName = await UploadImage(createAirline.AirlineImage!);

                airline.AirlineImageUrl = fileName;

                return await _airlineRepository.CreateAsync(airline);
            }
            catch(Exception ex)
            {
                return new GeneralReponse(false, $"Something went wrong: {ex.Message}");
            }


        }

        public async Task<GeneralReponse> DeleteAsync(int id)
        {
            return await _airlineRepository.DeleteAsync(id);    
        }

        public async Task<IEnumerable<GetAirlineDto>> GetAllAsync()
        {
            var airlines = await _airlineRepository.GetAllAsync();  
            var getAirlines = _mapper.Map<IEnumerable<GetAirlineDto>>(airlines);

            return getAirlines;
        }

        public async Task<GetAirlineDto?> GetByIdAsync(int id)
        {
            var airline = await _airlineRepository.GetByIdAsync(id);
            var getAirline = _mapper.Map<GetAirlineDto>(airline);

            return getAirline;  
        }

        public async Task<GeneralReponse> UpdateAsync(UpdateAirlineDto updateAirline)
        {
            var airline = _mapper.Map<Airline>(updateAirline);

            try
            {
                if (updateAirline.AirlineImage?.Length > 1 * 1024 * 1024)
                    return new GeneralReponse(false, "File size should not exceed 1 MB.");

                var fileName = await UploadImage(updateAirline.AirlineImage!);

                return await _airlineRepository.UpdateAsync(airline);
            }
            catch(Exception ex)
            {
                return new GeneralReponse(false, $"Something went wrong: {ex.Message}");
            }
        }


        private async Task<string> UploadImage(IFormFile imageName)
        {
            string[] allowedFileExtensions = [".jpg", ".jpeg", ".png",".svg"];

            string createdImage = await _fileService.SaveFileAsync(imageName, allowedFileExtensions);

            return createdImage;
        }
    }
}
