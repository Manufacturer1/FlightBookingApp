using BaseEntity.Entities;

namespace ServerLibrary.Decorator
{
    public class DiscountPriceCalculator : IPriceCalculator
    {
        private readonly IPriceCalculator _priceCalculator;
        private readonly Discount _discount;

        public DiscountPriceCalculator(IPriceCalculator priceCalculator, Discount discount)
        {
            _priceCalculator = priceCalculator;
            _discount = discount;
        }

        public decimal CalculatePrice()
        {
            var basePrice = _priceCalculator.CalculatePrice();

            if (_discount.IsActive)
            {
                return basePrice * (1 - (_discount.Percentage / 100m));
            }

            return basePrice;
        }
    }
}
