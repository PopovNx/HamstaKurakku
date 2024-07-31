using System.Text.Json.Serialization;

namespace HamsterCrack.Dto.Api.Tasks;

[Serializable]
public sealed record CheckTaskRequest(
    [property: JsonPropertyName("taskId")] string Id
);