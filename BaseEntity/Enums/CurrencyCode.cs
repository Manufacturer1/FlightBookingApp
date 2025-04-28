using System.Text.Json.Serialization;

namespace BaseEntity.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum CurrencyCode
    {
        USD,
        EUR,
        GBP,
        RON
    }
}
