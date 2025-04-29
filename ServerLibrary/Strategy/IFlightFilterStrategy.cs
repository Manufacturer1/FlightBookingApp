using BaseEntity.Dtos;
using BaseEntity.Entities;

namespace ServerLibrary.Strategy
{
    public interface IFlightFilterStrategy
    {
        IEnumerable<FlightCardResponseDto> FilterFlights(Itinerary itinerary, FlightCardRequestDto request);
    }
}
