using System.Text.Json.Serialization;

namespace HamsterCrack.Dto;

public sealed record BuyBoostRequest(
    [property: JsonPropertyName("boostId")]
    string BoostId,
    [property: JsonPropertyName("timestamp")]
    long Timestamp
)
{
    public static BuyBoostRequest Create(string boostId)
    {
        return new BuyBoostRequest(boostId, DateTimeOffset.UtcNow.ToUnixTimeSeconds());
    }
}