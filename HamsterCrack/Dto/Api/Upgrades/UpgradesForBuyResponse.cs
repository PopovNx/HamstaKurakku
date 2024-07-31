using System.Text.Json.Serialization;

namespace HamsterCrack.Dto.Api.Upgrades;

[Serializable]
public sealed record UpgradesForBuyResponse(
    [property: JsonPropertyName("upgradesForBuy")]
    UpgradeContent[] Upgrades
);