using System.Text.Json.Serialization;

namespace HamsterCrack.Dto.Api;

public sealed record ApiErrorResult(
    [property: JsonPropertyName("error_code")]
    string ErrorCode,
    [property: JsonPropertyName("error_message")]
    string Message
) : ApiResponse

{
public static ApiErrorResult CreateUnknownError() => new("UNKNOWN_ERROR", "An unknown error occurred.");
}