using System.Text.Json.Serialization;

namespace HamsterCrack.Dto.Api.Upgrades;

public sealed record BuyUpgradeRequest(
    [property: JsonPropertyName("upgradeId")]
    string UpgradeId,
    [property: JsonPropertyName("timestamp")]
    long Timestamp
)
{
    public static BuyUpgradeRequest Create(string boostId)
    {
        return new BuyUpgradeRequest(boostId, DateTimeOffset.UtcNow.ToUnixTimeSeconds());
    }
}