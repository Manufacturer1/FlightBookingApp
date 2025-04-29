using BaseEntity.Dtos;
using BaseEntity.Entities;

namespace ServerLibrary.Strategy
{
    public class FlightFilterContext
    {
        private IFlightFilterStrategy _strategy;

        public FlightFilterContext(IFlightFilterStrategy strategy)
        {
            _strategy = strategy;
        }
        public void SetStrategy(IFlightFilterStrategy strategy)
        {
            _strategy = strategy;
        }

        public IEnumerable<FlightCardResponseDto> FilterFlights(Itinerary itinerary, FlightCardRequestDto request)
        {
            return _strategy.FilterFlights(itinerary, request);
        }
    }
}
