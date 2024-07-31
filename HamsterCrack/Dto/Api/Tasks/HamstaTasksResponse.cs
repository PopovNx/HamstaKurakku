using System.Text.Json.Serialization;

namespace HamsterCrack.Dto.Api.Tasks;

public sealed record HamstaTasksResponse(
    [property: JsonPropertyName("tasks")] HamstaTask[] Tasks
);