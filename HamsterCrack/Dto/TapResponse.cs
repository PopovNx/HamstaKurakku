using System.Text.Json.Serialization;

namespace HamsterCrack.Dto;

public sealed record TapResponse(
    [property: JsonPropertyName("clickerUser")]
    HamstaState State);