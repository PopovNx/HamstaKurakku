using System.Text.Json.Serialization;

namespace HamsterCrack.Dto;

public sealed record HamstaTasksResponse(
    [property: JsonPropertyName("tasks")] HamstaTask[] Tasks
);