namespace HamsterCrack.Dto.Api;

public sealed record Empty
{
    public static Empty Instance { get; } = new();
}