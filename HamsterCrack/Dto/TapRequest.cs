using System.Text.Json.Serialization;

namespace HamsterCrack.Dto;

public sealed record TapRequest(
    [property: JsonPropertyName("count")] int Count,
    [property: JsonPropertyName("availableTaps")]
    int AvailableTaps,
    [property: JsonPropertyName("timestamp")]
    long Timestamp
)
{
    public static TapRequest CreateOptimal()
    {
        return new TapRequest(1000, 10000, DateTimeOffset.UtcNow.ToUnixTimeSeconds());
    }
}