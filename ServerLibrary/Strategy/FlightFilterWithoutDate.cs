using AutoMapper;
using BaseEntity.Dtos;
using BaseEntity.Entities;

namespace ServerLibrary.Strategy
{
    public class FlightFilterWithoutDate : IFlightFilterStrategy
    {
        private readonly IMapper _mapper;

        public FlightFilterWithoutDate(IMapper mapper)
        {
            _mapper = mapper;
        }

        public IEnumerable<FlightCardResponseDto> FilterFlights(Itinerary itinerary, FlightCardRequestDto request)
        {
            string classTypeString = request.ClassType.ToString();
            string tripTypeString = request.TripType.ToString();
            var segments = itinerary.Segments!.Where(x => x.IsReturnSegment == false);

            return segments
                .OrderBy(s => s.SegmentOrder)
                .Where(s => s.Flight != null &&
                            s.Flight.ClassType!.Equals(classTypeString, StringComparison.OrdinalIgnoreCase) &&
                            s.Flight.TripType.Equals(tripTypeString, StringComparison.OrdinalIgnoreCase))
                .Select(s => _mapper.Map<FlightCardResponseDto>(s.Flight));
        }
    }
}
