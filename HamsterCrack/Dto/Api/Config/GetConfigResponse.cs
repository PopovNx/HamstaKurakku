using System.Text.Json.Serialization;

namespace HamsterCrack.Dto.Api.Config;

public sealed record GetConfigResponse([property: JsonPropertyName("dailyCipher")]
CipherStatus Status);