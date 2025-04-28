using BaseEntity.Configurations;
using BaseEntity.Responses;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;

namespace ServerLibrary.Requests
{
    public class CurrencyRequest
    {
        private readonly string api_key;
        private readonly HttpClient _httpClient;

        public CurrencyRequest(HttpClient httpClient, IOptions<CurrencySettings> options)
        {
            api_key = options.Value.CURRENCY_API_KEY;
            _httpClient = httpClient;
        }

        public async Task<CurrencyAPIResult?> GetExchangeRates()
        {
            try
            {
                string url = $"https://v6.exchangerate-api.com/v6/{api_key}/latest/USD";

                return await _httpClient.GetFromJsonAsync<CurrencyAPIResult>(url);
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error fetching exchange rates: {ex.Message}");
                return null;
            }
        }

    }
}
