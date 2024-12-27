namespace SecondChance.Application.Models.Jwt;

public class JwtAccessTokenParseResult
{
    public bool IsValid { get; set; }
    public JwtAccessToken? Token { get; set; }
    public string? ErrorCode { get; set; }
}