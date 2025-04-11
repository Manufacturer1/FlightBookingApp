using AutoMapper;
using Azure.Core;
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
        // Also i need to handle the problem when there are multiple indirect flights associated with airlines
        // Currently we have 2 indirect flights Houston -> New York -> Los Angeles that correspond to airline 6.
        // What if we want to add more flights that has for exemple Houston -> Washington -> Los Angeles and airline 7 for exemple.
        // 
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

            var d =  directFlights;

            var indirectFlights = flights
                .Where(x => (x.Origin.Equals(flightCardRequest.Origin, StringComparison.OrdinalIgnoreCase) ||
                           x.Destination.Equals(flightCardRequest.Destination, StringComparison.OrdinalIgnoreCase)))
                .Where(x => !directFlights.Select(y => y.FlightNumber).Contains(x.FlightNumber))
                .Where(x => x.ClassType!.Equals(classTypeString, StringComparison.OrdinalIgnoreCase))
                .Where(x => x.DepartureDate.Date == flightCardRequest.DepartureDate.Date)
                .Where(x => !flightCardRequest.ReturnDate.HasValue ||
                           (x.ArrivalDate.Date == flightCardRequest.ReturnDate.Value.Date))
                .Where(x => x.TripType.Equals(tripTypeString, StringComparison.OrdinalIgnoreCase));



            var allItineraries = await _itineraryRepository.GetAllAsync();

            var requestedItineraries = allItineraries.Where(x => 
            x.Origin.Equals(flightCardRequest.Origin,StringComparison.OrdinalIgnoreCase) 
            && x.Destination.Equals(flightCardRequest.Destination,StringComparison.OrdinalIgnoreCase));

            var itinerariesToAdd = new List<Flight>();

            if (requestedItineraries.Any())
            {
                foreach (var itinerary in requestedItineraries)
                {
                    var itineraryFlights = itinerary.Segments?
                        .Where(s => s.Flight != null)
                        .OrderBy(s => s.SegmentOrder)
                        .Select(s => s.Flight)
                        .ToList();

                    if(itineraryFlights != null && itineraryFlights.Any())
                    { 
                        itinerariesToAdd.AddRange(itineraryFlights!);
                    }
                }

            }

            List<FlightCardResponseDto> allFlights = [.. _mapper.Map<IEnumerable<FlightCardResponseDto>>(directFlights), .. _mapper.Map<IEnumerable<FlightCardResponseDto>>(itinerariesToAdd)];
            allFlights = allFlights
                .OrderBy(x => x.Origin.Equals(flightCardRequest.Origin, StringComparison.OrdinalIgnoreCase)
                && x.Destination.Equals(flightCardRequest.Destination, StringComparison.OrdinalIgnoreCase)
                ? 1 : 0)
                 .ThenBy(x => x.DepartureDate) 
                 .ToList();

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

                if (updateFlight.BasePrice.HasValue)
                    originalFlight.BasePrice = updateFlight.BasePrice.Value;

                if (updateFlight.DepartureDate.HasValue)
                    originalFlight.DepartureDate = updateFlight.DepartureDate.Value;

                if (updateFlight.ArrivalDate.HasValue)
                    originalFlight.ArrivalDate = updateFlight.ArrivalDate.Value;

                if (updateFlight.DepartureTime != null)
                {
                    originalFlight.DepartureTime = updateFlight.DepartureTime;
                    if (TimeSpan.TryParse(updateFlight.DepartureTime, out var timeSpan))
                    {
                        originalFlight.TimeIcon = GetTime(timeSpan);
                    }
                }

                if (updateFlight.ArrivalTime != null)
                    originalFlight.ArrivalTime = updateFlight.ArrivalTime;

                return await _flightRepository.UpdateAsync(originalFlight);
            }
            catch (Exception ex)
            {
                return new GeneralReponse(false, $"Exception occurred: {ex.Message}");
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
