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

        public FlightService(IFlightRepository flightRepository, IFileService fileService)
        {
            _flightRepository = flightRepository;
            _fileService = fileService;
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

            List<FlightCardResponseDto> allFlights = [.. directFlights.Select(ToFlightCard), .. indirectFlights.Select(ToFlightCard)];

            return allFlights;
        }
        public async Task<GeneralReponse> AddAsync(CreateFlightDto createFlight)
        {
            try
            {
                if (createFlight.DestinationImage?.Length > 1 * 1024 * 1024)
                    return new GeneralReponse(false, "File size should not exceed 1 MB");

                var createdImageName = await UploadImage(createFlight.DestinationImage!);

                var flight = ToFlightEntity(createFlight,createdImageName);

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
            
            return flights.Select(ToGetFlightDto);
        }

        public async Task<GetFlightDto?> GetByFlightNumberAsync(string flightNumber)
        {
            var flight = await _flightRepository.GetByFlightNumberAsync(flightNumber);

            if (flight == null) return null;

            return ToGetFlightDto(flight);

        }


        public async Task<GeneralReponse> UpdateAsync(UpdateFlightDto updateFlight)
        {
            try
            {
                var originalFlight = await _flightRepository.GetByFlightNumberAsync(updateFlight.FlightNumber);
                if (originalFlight == null)
                    return new GeneralReponse(false, "Flight not found");

                string imageUrl = originalFlight.DestinationImageUrl;

                if (updateFlight.DestinationImage != null)
                {
                    imageUrl = await UploadImage(updateFlight.DestinationImage);

                    _fileService.DeleteFile(originalFlight.DestinationImageUrl);
                }

                var flight = FromUpdateToFlightEntity(updateFlight, imageUrl);

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

        #region MAPPINGS
        private Flight ToFlightEntity(CreateFlightDto model,string imageName)
        {
            return new Flight
            {
                ClassType = model.ClassType,
                ArrivalDate = model.ArrivalDate,
                AvailableSeats = model.AvailableSeats,
                ArrivalTime = model.ArrivalTime,
                BasePrice = model.BasePrice,
                DepartureDate = model.DepartureDate,
                DepartureTime = model.DepartureTime,
                Destination = model.Destination,    
                DestinationImageUrl = imageName,
                Origin = model.Origin,  
                TotalSeats = model.TotalSeats,
                TripType = model.TripType,
            };
        }
        private Flight FromUpdateToFlightEntity(UpdateFlightDto updateFlight,string imageUrl)
        {
            return new Flight
            {
                ClassType = updateFlight.ClassType,
                ArrivalDate = updateFlight.ArrivalDate,
                ArrivalTime= updateFlight.ArrivalTime,
                AvailableSeats= updateFlight.AvailableSeats,
                BasePrice= updateFlight.BasePrice,
                DepartureDate= updateFlight.DepartureDate,
                DepartureTime= updateFlight.DepartureTime,  
                Destination= updateFlight.Destination,
                DestinationImageUrl = imageUrl,
                FlightNumber = updateFlight.FlightNumber,
                Origin= updateFlight.Origin,
                TotalSeats = updateFlight.TotalSeats,
                TripType= updateFlight.TripType,
            };
        }
        private GetFlightDto ToGetFlightDto(Flight flight)
        {
            return new GetFlightDto
            {
                ClassType = flight.ClassType,
                ArrivalDate = flight.ArrivalDate,
                ArrivalTime = flight.ArrivalTime,
                AvailableSeats = flight.AvailableSeats,
                BasePrice = flight.BasePrice,
                DepartureDate = flight.DepartureDate,
                DepartureTime= flight.DepartureTime,
                Destination = flight.Destination,
                Origin= flight.Origin,
                DestinationImageUrl= flight.DestinationImageUrl,
                FlightNumber = flight.FlightNumber,
                TimeIcon= flight.TimeIcon,  
                TotalSeats = flight.TotalSeats,
                TripType= flight.TripType,
            };
        }
        private FlightCardResponseDto ToFlightCard(Flight flight)
        {
            return new FlightCardResponseDto
            {
                FlightNumber= flight.FlightNumber,
                ArrivalDate= flight.ArrivalDate,
                ArrivalTime= flight.ArrivalTime,
                BasePrice= flight.BasePrice,
                DepartureDate= flight.DepartureDate,
                DepartureTime = flight.DepartureTime,
                Destination= flight.Destination,
                DestinationImageUrl = flight.DestinationImageUrl,
                Origin = flight.Origin,
                TimeIcon = flight.TimeIcon,
                TripType = flight.TripType,
            };
        }
        #endregion

    }
}
