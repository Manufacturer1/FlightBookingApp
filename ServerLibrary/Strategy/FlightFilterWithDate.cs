using AutoMapper;
using BaseEntity.Dtos;
using BaseEntity.Entities;

namespace ServerLibrary.Strategy
{
    public class FlightFilterWithDate : IFlightFilterStrategy
    {
        private readonly IMapper _mapper;

        public FlightFilterWithDate(IMapper mapper)
        {
            _mapper = mapper;
        }
        public IEnumerable<FlightCardResponseDto> FilterFlights(Itinerary itinerary, FlightCardRequestDto request)
        {
            string classTypeString = request.ClassType.ToString();
            string tripTypeString = request.TripType.ToString();
            bool isDate = itinerary.DepartureDate.Date == request.DepartureDate.Date && itinerary.ArrivalDate.Date == request.ReturnDate!.Value.Date;

            var segments = itinerary
                .Segments!.Where(x => x.IsReturnSegment == false);

            return segments
                .OrderBy(s => s.SegmentOrder)
                .Where(s => s.Flight != null &&
                            s.Flight.ClassType!.Equals(classTypeString, StringComparison.OrdinalIgnoreCase) &&
                            s.Flight.DepartureDate.Date == request.DepartureDate.Date &&
                            isDate &&
                            s.Flight.TripType.Equals(tripTypeString, StringComparison.OrdinalIgnoreCase))
                .Select(s => _mapper.Map<FlightCardResponseDto>(s.Flight));
        }
    }
}
