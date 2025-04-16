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
        private readonly IItineraryRepository _itineraryRepository;

        public FlightService(IFlightRepository flightRepository, IFileService fileService,IMapper mapper,IItineraryRepository itineraryRepository)
        {
            _flightRepository = flightRepository;
            _fileService = fileService;
            _mapper = mapper;
            _itineraryRepository = itineraryRepository;
        }

        public async Task<IEnumerable<ItineraryCardResponseDto>> GetFlightCards(FlightCardRequestDto flightCardRequest, bool withoutDate = false)
        {
            var allItineraries = await _itineraryRepository.GetAllAsync();

            var searchedItineraries = allItineraries.Where(x =>
                x.Origin.Equals(flightCardRequest.Origin, StringComparison.OrdinalIgnoreCase) &&
                x.Destination.Equals(flightCardRequest.Destination, StringComparison.OrdinalIgnoreCase));

            string classTypeString = flightCardRequest.ClassType.ToString();
            string tripTypeString = flightCardRequest.TripType.ToString();

            var result = new List<ItineraryCardResponseDto>();

            foreach (var itinerary in searchedItineraries)
            {
                IEnumerable<FlightCardResponseDto> matchingFlights;

                if (withoutDate)
                {
                    
                    matchingFlights = itinerary.Segments!
                        .OrderBy(s => s.SegmentOrder)
                        .Where(s => s.Flight != null &&
                                    s.Flight.ClassType!.Equals(classTypeString, StringComparison.OrdinalIgnoreCase) &&
                                    s.Flight.TripType.Equals(tripTypeString, StringComparison.OrdinalIgnoreCase))
                        .Select(s => _mapper.Map<FlightCardResponseDto>(s.Flight))
                        .ToList();
                }
                else
                {
                  
                    matchingFlights = itinerary.Segments!
                        .OrderBy(s => s.SegmentOrder)
                        .Where(s => s.Flight != null &&
                                    s.Flight.ClassType!.Equals(classTypeString, StringComparison.OrdinalIgnoreCase) &&
                                    s.Flight.DepartureDate.Date == flightCardRequest.DepartureDate.Date &&
                                    s.Flight.TripType.Equals(tripTypeString, StringComparison.OrdinalIgnoreCase))
                        .Where(s => !flightCardRequest.ReturnDate.HasValue ||
                                    s.Flight!.ArrivalDate.Date == flightCardRequest.ReturnDate.Value.Date)
                        .Select(s => _mapper.Map<FlightCardResponseDto>(s.Flight))
                        .ToList();
                }

                if (matchingFlights.Any())
                {
                    var mappedIinerary = _mapper.Map<GetItineraryDto>(itinerary);

                    var card = new ItineraryCardResponseDto
                    {
                        Itinerary = mappedIinerary,
                        Flights = matchingFlights.ToList(),
                    };
                    result.Add(card);
                }
            }

            return result;
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
                flight.TimeIcon = "morning";
              

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

           
                if (updateFlight.DestinationImage != null)
                {
                    var imageUrl = await UploadImage(updateFlight.DestinationImage);
                    _fileService.DeleteFile(originalFlight.DestinationImageUrl);
                    originalFlight.DestinationImageUrl = imageUrl;
                }
                originalFlight.PlaneId = updateFlight.PlaneId;

                if (updateFlight.ClassType != null)
                    originalFlight.ClassType = updateFlight.ClassType;

                if (updateFlight.TripType != null)
                    originalFlight.TripType = updateFlight.TripType;

                if (updateFlight.Origin != null)
                    originalFlight.Origin = updateFlight.Origin;

                if (updateFlight.Destination != null)
                    originalFlight.Destination = updateFlight.Destination;

                if (updateFlight.TotalSeats.HasValue)
                    originalFlight.TotalSeats = updateFlight.TotalSeats.Value;

                if (updateFlight.AvailableSeats.HasValue)
                    originalFlight.AvailableSeats = updateFlight.AvailableSeats.Value;


                if (updateFlight.DepartureDate.HasValue)
                    originalFlight.DepartureDate = updateFlight.DepartureDate.Value;

                if (updateFlight.ArrivalDate.HasValue)
                    originalFlight.ArrivalDate = updateFlight.ArrivalDate.Value;



                if (updateFlight.ArrivalTime != null)
                    originalFlight.ArrivalTime = updateFlight.ArrivalTime;

                return await _flightRepository.UpdateAsync(originalFlight);
            }
            catch (Exception ex)
            {
                return new GeneralReponse(false, $"Exception occurred: {ex.Message}");
            }
        }
        private async Task<string> UploadImage(IFormFile imageName)
        {
            string[] allowedFileExtensions = [".jpg", ".jpeg", ".png"];
            string createdImageName = await _fileService.SaveFileAsync(imageName, allowedFileExtensions);

            return createdImageName;
        }

    }
}
