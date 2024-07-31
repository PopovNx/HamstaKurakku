using System.Text.Json.Serialization;

namespace HamsterCrack.Dto.Api.Tasks;

public sealed record HamstaTask(
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("isCompleted")]
    bool IsCompleted
);