using BaseEntity.Enums;

namespace ServerLibrary.Flyweight
{
    public class CurrencyFactory
    {
        private readonly Dictionary<CurrencyCode, CurrencyFlyweight> _cache = new();


        public CurrencyFlyweight GetCurrency(CurrencyCode currencyCode)
        {
            if (_cache.TryGetValue(currencyCode, out var flyweight))
            {
                return flyweight;
            }
            var newFlyweight = CreateFlyweight(currencyCode);
            _cache.Add(currencyCode, newFlyweight);
            return newFlyweight;
        }
        private CurrencyFlyweight CreateFlyweight(CurrencyCode currencyCode)
        {
            return currencyCode switch
            {
                CurrencyCode.USD => new CurrencyFlyweight(currencyCode, "$", 2),
                CurrencyCode.EUR => new CurrencyFlyweight(currencyCode, "€", 2),
                CurrencyCode.GBP => new CurrencyFlyweight(currencyCode, "£", 2),
                CurrencyCode.RON => new CurrencyFlyweight(currencyCode, "lei", 2),
                _ => throw new ArgumentException("Unsuported currency")
            };
        }
    }
}
