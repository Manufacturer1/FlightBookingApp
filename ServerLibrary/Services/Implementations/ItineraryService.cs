using AutoMapper;
using BaseEntity.Dtos;
using BaseEntity.Entities;
using BaseEntity.Responses;
using ServerLibrary.Repositories.Interfaces;
using ServerLibrary.Services.Interfaces;

namespace ServerLibrary.Services.Implementations
{
    public class ItineraryService : IItineraryService
    {
        private readonly IItineraryRepository _itineraryRepository;
        private readonly IFlightRepository _flightRepository;
        private readonly IMapper _mapper;

        public ItineraryService(IItineraryRepository itineraryRepository,IFlightRepository flightRepository,IMapper mapper)
        {
            _flightRepository = flightRepository;
            _mapper = mapper;
            _itineraryRepository = itineraryRepository;
        }

        public async Task<GeneralReponse> AddAsync(CreateItineraryDto itineraryDto)
        {
            if (itineraryDto == null)
                return new GeneralReponse(false, "Itinerary data is required");

            if (itineraryDto.Segments == null || !itineraryDto.Segments.Any())
                return new GeneralReponse(false, "At least one flight segment is required");

            var flights = new List<Flight>();
            foreach (var segment in itineraryDto.Segments)
            {
                var flight = await _flightRepository.GetByFlightNumberAsync(segment.FlightNumber);
                if (flight == null)
                    return new GeneralReponse(false, $"Flight {segment.FlightNumber} was not found");


                flights.Add(flight);
            }

            var itinerary = _mapper.Map<Itinerary>(itineraryDto);

            itinerary.Segments = flights.Select((flight, index) => new FlightSegment
            {
                FlightNumber = flight.FlightNumber,
                SegmentOrder = index + 1,
            }).ToList();


            await _itineraryRepository.AddAsync(itinerary);
            return new GeneralReponse(true, $"Itinerary {itinerary.Id} was created successfully");
            
        }

        public async Task<IEnumerable<GetItineraryDto>> GetAllAsync()
        {
            var itineraries = await _itineraryRepository.GetAllAsync();

            return _mapper.Map<IEnumerable<GetItineraryDto>>(itineraries);
        }
    }
}
