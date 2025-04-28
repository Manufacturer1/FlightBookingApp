namespace BaseEntity.Responses
{
    public class CurrencyResponse
    {
        public decimal Amount { get; set; }
        public string FromCurrency { get; set; } = string.Empty;
        public string ToCurrency { get; set; } = string.Empty;
        public decimal ConvertedAmount { get; set; }
        public decimal ExchangeRate { get; set; }
        public string FormatedConvertedAmount { get; set; } = string.Empty;
    }
}
