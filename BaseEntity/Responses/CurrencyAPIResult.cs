using BaseEntity.Models;

namespace BaseEntity.Responses
{
    public class CurrencyAPIResult
    {
        public string result { get; init; } = string.Empty;
        public string base_code { get; init; } = string.Empty;
        public Rates conversion_rates { get; init; } = new Rates();
    }
}
