using BaseEntity.Entities;

namespace ServerLibrary.Decorator
{
    public class BasePriceCalculator : IPriceCalculator
    {
        private readonly Flight _flight;

        public BasePriceCalculator(Flight flight)
        {
            _flight = flight;
        }

        public decimal CalculatePrice()
        {
            return _flight.AvailableSeats > (_flight.TotalSeats * 0.2)
                ? _flight.BasePrice : _flight.BasePrice * 1.2m;
        }
    }
}
