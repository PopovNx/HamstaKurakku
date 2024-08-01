using System.Text.Json.Serialization;

namespace HamsterCrack.Dto;

[Serializable]
public sealed record HamstaState
{
    [JsonPropertyName("id")] public required string Id { get; set; }
    [JsonPropertyName("totalCoins")] public required double TotalCoins { get; set; }
    [JsonPropertyName("balanceCoins")] public required double BalanceCoins { get; set; }
    [JsonPropertyName("level")] public required int Level { get; set; }
    [JsonPropertyName("availableTaps")] public required int AvailableTaps { get; set; }
    [JsonPropertyName("earnPassivePerHour")] public required int EarnPassivePerHour { get; set; }
}