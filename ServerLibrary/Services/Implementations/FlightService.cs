using AutoMapper;
using BaseEntity.Dtos;
using BaseEntity.Entities;
using BaseEntity.Responses;
using Microsoft.AspNetCore.Http;
using ServerLibrary.Repositories.Interfaces;
using ServerLibrary.Services.Interfaces;

namespace ServerLibrary.Services.Implementations
{
    public class FlightService : IFlightService
    {
        private readonly IFlightRepository _flightRepository;
        private readonly IFileService _fileService;
        private readonly IMapper _mapper;

        public FlightService(IFlightRepository flightRepository, IFileService fileService,IMapper mapper)
        {
            _flightRepository = flightRepository;
            _fileService = fileService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<FlightCardResponseDto>> GetFlightCards(FlightCardRequestDto flightCardRequest)
        {
            var flights = await _flightRepository.GetAllAsync();


            string classTypeString = flightCardRequest.ClassType.ToString();
            string tripTypeString = flightCardRequest.TripType.ToString();

            var directFlights = flights
                .Where(x => x.ClassType!.Equals(classTypeString, StringComparison.OrdinalIgnoreCase))
                .Where(x => x.Origin.Equals(flightCardRequest.Origin, StringComparison.OrdinalIgnoreCase))
                .Where(x => x.Destination.Equals(flightCardRequest.Destination, StringComparison.OrdinalIgnoreCase))
                .Where(x => x.DepartureDate.Date == flightCardRequest.DepartureDate.Date)
                .Where(x => !flightCardRequest.ReturnDate.HasValue ||
                           (x.ArrivalDate.Date == flightCardRequest.ReturnDate.Value.Date))
                .Where(x => x.TripType.Equals(tripTypeString, StringComparison.OrdinalIgnoreCase));

            var indirectFlights = flights
                .Where(x => (x.Origin.Equals(flightCardRequest.Origin, StringComparison.OrdinalIgnoreCase) ||
                           x.Destination.Equals(flightCardRequest.Destination, StringComparison.OrdinalIgnoreCase)))
                .Where(x => !directFlights.Select(y => y.FlightNumber).Contains(x.FlightNumber))
                .Where(x => x.ClassType!.Equals(classTypeString, StringComparison.OrdinalIgnoreCase))
                .Where(x => x.DepartureDate.Date == flightCardRequest.DepartureDate.Date)
                .Where(x => !flightCardRequest.ReturnDate.HasValue ||
                           (x.ArrivalDate.Date == flightCardRequest.ReturnDate.Value.Date))
                .Where(x => x.TripType.Equals(tripTypeString, StringComparison.OrdinalIgnoreCase));
            


            List<FlightCardResponseDto> allFlights = [.. _mapper.Map<IEnumerable<FlightCardResponseDto>>(directFlights), .. _mapper.Map<IEnumerable<FlightCardResponseDto>>(indirectFlights)];

            return allFlights;
        }
        public async Task<GeneralReponse> AddAsync(CreateFlightDto createFlight)
        {
            try
            {
                if (createFlight.DestinationImage?.Length > 1 * 1024 * 1024)
                    return new GeneralReponse(false, "File size should not exceed 1 MB");

                var createdImageName = await UploadImage(createFlight.DestinationImage!);

                var flight = _mapper.Map<Flight>(createFlight);

                flight.DestinationImageUrl = createdImageName;

                if (TimeSpan.TryParse(flight.DepartureTime, out var timeSpan))
                    flight.TimeIcon = GetTime(timeSpan);
                
                else flight.TimeIcon = "morning";



                return await _flightRepository.CreateAsync(flight);
            }
            catch(Exception ex)
            {
                return new GeneralReponse(false, $"Something went wrong: {ex.Message}");
            }

        }

        public async Task<GeneralReponse> DeleteAsync(string flightNumber)
        {
            return await _flightRepository.DeleteAsync(flightNumber);
        }

        public async Task<IEnumerable<GetFlightDto>> GetAllAsync()
        {
           var flights = await _flightRepository.GetAllAsync();
            
            return _mapper.Map<IEnumerable<GetFlightDto>>(flights);
        }

        public async Task<GetFlightDto?> GetByFlightNumberAsync(string flightNumber)
        {
            var flight = await _flightRepository.GetByFlightNumberAsync(flightNumber);

            if (flight == null) return null;

            return _mapper.Map<GetFlightDto>(flight);

        }


        public async Task<GeneralReponse> UpdateAsync(UpdateFlightDto updateFlight)
        {
            try
            {
                var originalFlight = await _flightRepository.GetByFlightNumberAsync(updateFlight.FlightNumber);
                if (originalFlight == null)
                    return new GeneralReponse(false, "Flight not found");

                string imageUrl = originalFlight.DestinationImageUrl;
                var flight = _mapper.Map<Flight>(originalFlight);

                if (updateFlight.DestinationImage != null)
                {
                    imageUrl = await UploadImage(updateFlight.DestinationImage);

                    _fileService.DeleteFile(originalFlight.DestinationImageUrl);
                    flight.DestinationImageUrl = imageUrl;
                }


                return await _flightRepository.UpdateAsync(flight);

            }
            catch (Exception ex)
            {
                return new GeneralReponse(false, $"Exception occured: {ex.Message}");
            }
        }

        private string GetTime(TimeSpan timeSpan)
        {
            if(timeSpan >= new TimeSpan(6,0,0) && timeSpan < new TimeSpan(18, 0, 0))
            {
                return "morning";
            }
            return "night";
        }
        private async Task<string> UploadImage(IFormFile imageName)
        {
            string[] allowedFileExtensions = [".jpg", ".jpeg", ".png"];
            string createdImageName = await _fileService.SaveFileAsync(imageName, allowedFileExtensions);

            return createdImageName;
        }

    }
}
