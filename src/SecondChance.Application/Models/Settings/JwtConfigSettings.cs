namespace SecondChance.Application.Models.Settings;

public class JwtConfigSettings
{
    public string Key { get; init; } = null!;
    public string Issuer { get; init; } = null!;
    public string Audience { get; init; } = null!;
    public long AccessTokenExpirationMilliseconds { get; init; }
    public long RefreshTokenExpirationMilliseconds { get; init; }
}