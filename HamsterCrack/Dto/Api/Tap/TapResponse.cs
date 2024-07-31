using System.Text.Json.Serialization;

namespace HamsterCrack.Dto.Api.Tap;

public sealed record TapResponse(
    [property: JsonPropertyName("clickerUser")]
    HamstaState State);