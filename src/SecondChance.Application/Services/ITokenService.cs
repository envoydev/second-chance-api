using SecondChance.Application.Models;
using SecondChance.Domain.Enums;

namespace SecondChance.Application.Services;

public interface ITokenService
{
    string GenerateAccessToken(Guid userId, Role role);
    JwtRefreshToken GenerateRefreshToken();
    JwtAccessToken? ParseAccessToken(string accessToken);
}