namespace SecondChance.Application.Models;

public class JwtRefreshToken
{
    public string Token { get; init; } = null!;
    public DateTime ExpiredAt { get; init; }
}