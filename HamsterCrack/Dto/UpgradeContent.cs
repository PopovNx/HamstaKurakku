using System.Text.Json.Serialization;

namespace HamsterCrack.Dto;

[Serializable]
public sealed record UpgradeContent(
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("price")] double Price,
    [property: JsonPropertyName("profitPerHour")]
    double ProfitPerHour,
    [property: JsonPropertyName("currentProfitPerHour")]
    double CurrentProfitPerHour,
    [property: JsonPropertyName("profitPerHourDelta")]
    double ProfitPerHourDelta,
    [property: JsonPropertyName("isAvailable")]
    bool IsAvailable,
    [property: JsonPropertyName("isExpired")]
    bool IsExpired,
    [property: JsonPropertyName("section")]
    string Section
)
{
    public double ProfitPerCostRate => ProfitPerHour / Price;
}