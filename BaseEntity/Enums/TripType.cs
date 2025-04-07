using System.Text.Json.Serialization;

namespace ServerLibrary.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum TripType
    {
        OneWay,
        RoundTrip
    }
}
