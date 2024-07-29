using System.Text.Json.Serialization;

namespace HamsterCrack.Dto;

[Serializable]
public sealed record DoDailyCipherRequest(
    [property: JsonPropertyName("cipher")] string Cipher
);