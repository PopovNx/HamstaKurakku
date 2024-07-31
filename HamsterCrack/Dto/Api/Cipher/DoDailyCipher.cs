using System.Text.Json.Serialization;

namespace HamsterCrack.Dto.Api.Cipher;

[Serializable]
public sealed record DoDailyCipherRequest(
    [property: JsonPropertyName("cipher")] string Cipher
);