using System.Text.Json.Serialization;

namespace HamsterCrack.Dto;

public sealed record HamstaTask(
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("isCompleted")]
    bool IsCompleted
);