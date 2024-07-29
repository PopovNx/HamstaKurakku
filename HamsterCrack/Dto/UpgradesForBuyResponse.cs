using System.Text.Json.Serialization;

namespace HamsterCrack.Dto;

[Serializable]
public sealed record UpgradesForBuyResponse(
    [property: JsonPropertyName("upgradesForBuy")]
    UpgradeContent[] Upgrades
);