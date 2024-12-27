using SecondChance.Application.Models;
using SecondChance.Application.Models.Jwt;
using SecondChance.Domain.Enums;

namespace SecondChance.Application.Services;

public interface ITokenService
{
    string GenerateAccessToken(Guid userId, Role role);
    JwtRefreshToken GenerateRefreshToken();
    JwtAccessTokenParseResult ParseAccessToken(string accessToken);
}