using System.Text.Json.Serialization;

namespace HamsterCrack.Dto;

[Serializable]
public sealed record CheckTaskRequest(
    [property: JsonPropertyName("taskId")] string Id
);