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

            var itinerary = _mapper.Map<Itinerary>(itineraryDto);
            itinerary.Segments = new List<FlightSegment>();

            int outboundOrder = 1;
            int returnOrder = 1;

            foreach (var segmentDto in itineraryDto.Segments)
            {
                var flight = await _flightRepository.GetByFlightNumberAsync(segmentDto.FlightNumber);
                if (flight == null)
                    return new GeneralReponse(false, $"Flight {segmentDto.FlightNumber} was not found");

                var segment = new FlightSegment
                {
                    FlightNumber = flight.FlightNumber,
                    IsReturnSegment = segmentDto.IsReturnSegment,
                    SegmentOrder = segmentDto.IsReturnSegment ? returnOrder++ : outboundOrder++
                };

                itinerary.Segments.Add(segment);
            }

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
