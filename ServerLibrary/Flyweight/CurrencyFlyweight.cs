using BaseEntity.Enums;

namespace ServerLibrary.Flyweight
{
    public class CurrencyFlyweight
    {
        public CurrencyCode Code { get; } = CurrencyCode.USD;
        public string Symbol { get; }
        public int DecimalPlaces { get; }

        public CurrencyFlyweight(CurrencyCode code, string symbol, int decimalPlaces)
        {
            Code = code;
            Symbol = symbol;
            DecimalPlaces = decimalPlaces;
        }

        public decimal ConvertFromUsd(decimal usdAmount, decimal currentUsdRate)
        {
            return Math.Round(usdAmount * currentUsdRate, DecimalPlaces);
        }

        public string Format(decimal amount)
        {
            return $"{amount.ToString($"N{DecimalPlaces}")}{Symbol}";
        }
    }
}
