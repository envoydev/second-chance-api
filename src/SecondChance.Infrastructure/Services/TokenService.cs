using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Ardalis.GuardClauses;
using Microsoft.IdentityModel.Tokens;
using SecondChance.Application.Constants;
using SecondChance.Application.Logger;
using SecondChance.Application.Models;
using SecondChance.Application.Services;
using SecondChance.Domain.Enums;

namespace SecondChance.Infrastructure.Services;

internal class TokenService : ITokenService
{
    private readonly IDateTimeService _dateTimeService;
    private readonly IApplicationLogger<TokenService> _logger;
    private readonly ISettingsService _settingsService;

    public TokenService(IApplicationLogger<TokenService> logger,
        ISettingsService settingsService,
        IDateTimeService dateTimeService)
    {
        _logger = logger;
        _settingsService = settingsService;
        _dateTimeService = dateTimeService;
    }

    public string GenerateAccessToken(Guid userId, Role role)
    {
        var jwtConfig = _settingsService.GetSettings().JwtConfig;
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(jwtConfig.Key);

        var issuedDateTime = _dateTimeService.GetUtc();
        var expirationDateTime = issuedDateTime.AddMilliseconds(jwtConfig.AccessTokenExpirationMilliseconds);

        var claims = new[]
        {
            new Claim(TokenConstants.UserId, userId.ToString()),
            new Claim(TokenConstants.Role, role.ToString()),
            new Claim(TokenConstants.IssuedAt, _dateTimeService.FromDateTimeToUnixTimeStamp(issuedDateTime).ToString()),
            new Claim(TokenConstants.ExpiredAt, _dateTimeService.FromDateTimeToUnixTimeStamp(expirationDateTime).ToString())
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }

    public JwtRefreshToken GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        string token;

        using (var randomNumberGenerator = RandomNumberGenerator.Create())
        {
            randomNumberGenerator.GetBytes(randomNumber);
            token = Convert.ToBase64String(randomNumber);
        }

        var jwtConfig = _settingsService.GetSettings().JwtConfig;

        return new JwtRefreshToken
        {
            Token = token,
            ExpiredAt = _dateTimeService.GetUtc().AddMilliseconds(jwtConfig.RefreshTokenExpirationMilliseconds)
        };
    }

    public JwtAccessToken? ParseAccessToken(string accessToken)
    {
        try
        {
            var jwtConfig = _settingsService.GetSettings().JwtConfig;
            var key = Encoding.ASCII.GetBytes(jwtConfig.Key);

            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero,
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };

            tokenHandler.ValidateToken(accessToken, tokenValidationParameters, out var validatedToken);

            var jwtSecurityToken = (JwtSecurityToken)validatedToken;

            var userId = Guard.Against.NullOrWhiteSpace(
                jwtSecurityToken.Claims.FirstOrDefault(x => x.Type == TokenConstants.UserId)?.Value,
                message: $"'{TokenConstants.UserId}' cannot be null or empty.");

            var role = Guard.Against.NullOrWhiteSpace(
                jwtSecurityToken.Claims.FirstOrDefault(x => x.Type == TokenConstants.Role)?.Value,
                message: $"'{TokenConstants.Role}' cannot be null or empty.");

            Guard.Against.Expression(result => result == false, Enum.TryParse(role, out Role roleEnum),
                $"Cannot parse enum for '{TokenConstants.Role}'.");

            var issuedAt = Guard.Against.NullOrWhiteSpace(
                jwtSecurityToken.Claims.FirstOrDefault(x => x.Type == TokenConstants.IssuedAt)?.Value,
                message: $"'{TokenConstants.IssuedAt}' cannot be null or empty.");

            var expiredAt = Guard.Against.NullOrWhiteSpace(
                jwtSecurityToken.Claims.FirstOrDefault(x => x.Type == TokenConstants.ExpiredAt)?.Value,
                message: $"'{TokenConstants.ExpiredAt}' cannot be null or empty.");

            var jwtToken = new JwtAccessToken
            {
                UserId = Guid.Parse(userId),
                Role = roleEnum,
                IssuedAtUnixTimeStamp = int.Parse(issuedAt),
                ExpiredAtUnixTimeStamp = int.Parse(expiredAt)
            };

            return jwtToken;
        }
        catch (Exception exception)
        {
            _logger.LogWarning(exception, "Exception during token verifying");

            return null;
        }
    }
}