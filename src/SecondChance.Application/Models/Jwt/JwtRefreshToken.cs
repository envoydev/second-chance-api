namespace SecondChance.Application.Models.Jwt;

public class JwtRefreshToken
{
    public string Token { get; init; } = null!;
    public DateTime ExpiredAt { get; init; }
}