namespace HamsterCrack.Dto.Api;

public sealed record ApiSuccessResponse<T>(T Data) : ApiResponse;