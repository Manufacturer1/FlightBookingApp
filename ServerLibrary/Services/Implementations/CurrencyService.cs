using BaseEntity.Enums;
using BaseEntity.Responses;
using ServerLibrary.Flyweight;
using ServerLibrary.Requests;
using ServerLibrary.Services.Interfaces;

namespace ServerLibrary.Services.Implementations
{
    public class CurrencyService : ICurrencyService
    {
        private readonly CurrencyFactory _currencyFactory;
        private readonly CurrencyRequest _currencyRequest;

        public CurrencyService(CurrencyFactory currencyFactory, CurrencyRequest currencyRequest)
        {
            _currencyFactory = currencyFactory;
            _currencyRequest = currencyRequest;
        }
        public async Task<CurrencyResponse> ConvertAmount(decimal amount, CurrencyCode to)
        {

            var currencyExchangeResponse = await _currencyRequest.GetExchangeRates();

            if (currencyExchangeResponse == null || currencyExchangeResponse.result != "success")
            {
                throw new Exception("Conversion failed due to an api call error.");
            }

            decimal rate = GetRateFromResult(currencyExchangeResponse, to);

            var currency = _currencyFactory.GetCurrency(to);
            decimal convertedAmount = currency.ConvertFromUsd(amount, rate);
            var formatedCurrency = currency.Format(convertedAmount);

            var currencyResponse = new CurrencyResponse
            {
                Amount = amount,
                ConvertedAmount = convertedAmount,
                FromCurrency = "USD",
                ToCurrency = to.ToString(),
                ExchangeRate = rate,
                FormatedConvertedAmount = formatedCurrency
            };

            return currencyResponse;
        }

        private decimal GetRateFromResult(CurrencyAPIResult result, CurrencyCode code)
        {
            return code switch
            {
                CurrencyCode.USD => result.conversion_rates.USD,
                CurrencyCode.EUR => result.conversion_rates.EUR,
                CurrencyCode.GBP => result.conversion_rates.GBP,
                CurrencyCode.RON => result.conversion_rates.RON,
                _ => throw new ArgumentException("Unsupported currency")
            };
        }
    }
}
