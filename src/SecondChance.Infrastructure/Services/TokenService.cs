using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Ardalis.GuardClauses;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using SecondChance.Application.Constants;
using SecondChance.Application.Errors;
using SecondChance.Application.Logger;
using SecondChance.Application.Models.Jwt;
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
            Audience = jwtConfig.Audience,
            Issuer = jwtConfig.Issuer,
            IssuedAt = issuedDateTime,
            Expires = expirationDateTime,
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
    
    public JwtAccessTokenParseResult ParseAccessToken(string accessToken)
    {
        try
        {
            Guard.Against.NullOrWhiteSpace(accessToken, message: $"'{accessToken}' cannot be null or empty.");
            
            var jwtConfig = _settingsService.GetSettings().JwtConfig;
            var key = Encoding.ASCII.GetBytes(jwtConfig.Key);

            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                ValidIssuer = jwtConfig.Issuer,
                ValidAudience = jwtConfig.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };

            tokenHandler.ValidateToken(accessToken, tokenValidationParameters, out var validatedToken);

            var jwtSecurityToken = (JwtSecurityToken)validatedToken;

            var userId = GetRequiredClaim(jwtSecurityToken, TokenConstants.UserId); 
      
            var role = GetRequiredClaim(jwtSecurityToken, TokenConstants.Role); 
            Guard.Against.Expression(result => result == false, Enum.TryParse(role, out Role roleEnum),
                $"Cannot parse enum for '{TokenConstants.Role}'.");

            var issuedAt = GetRequiredClaim(jwtSecurityToken, TokenConstants.IssuedAt);
            Guard.Against.Expression(result => result == false, int.TryParse(issuedAt, out var issuedAtTimestamp),
                $"'{TokenConstants.IssuedAt}' is not a valid Unix timestamp.");

            var expiredAt = GetRequiredClaim(jwtSecurityToken, TokenConstants.ExpiredAt);
            Guard.Against.Expression(result => result == false, int.TryParse(expiredAt, out var expiredAtTimestamp),
                $"'{TokenConstants.ExpiredAt}' is not a valid Unix timestamp.");
            
            return new JwtAccessTokenParseResult
            {
                IsValid = true,
                Token = new JwtAccessToken
                {
                    UserId = Guid.Parse(userId),
                    Role = roleEnum,
                    IssuedAtUnixTimeStamp = issuedAtTimestamp,
                    ExpiredAtUnixTimeStamp = expiredAtTimestamp
                }
            };
        }
        catch (Exception exception) when (TryMapException(exception, out var errorCode, out var logLevel))
        {
            LogException(exception, logLevel, errorCode);
            
            return new JwtAccessTokenParseResult
            {
                IsValid = false,
                ErrorCode = errorCode
            };
        }
    }

    #region Private methods

    private static string GetRequiredClaim(JwtSecurityToken jwtSecurityToken, string claimType)
    {
        var claimValue = jwtSecurityToken.Claims.FirstOrDefault(x => x.Type == claimType)?.Value;
        Guard.Against.NullOrWhiteSpace(claimValue, message: $"'{claimType}' cannot be null or empty.");
        return claimValue;
    }

    private static bool TryMapException(Exception exception, out string errorCode, out LogLevel logLevel)
    {
        switch (exception)
        {
            case SecurityTokenExpiredException:
                errorCode = ErrorMessageCodes.TokenHasExpired;
                logLevel = LogLevel.Information; // Just log this as info
                return true;
            case SecurityTokenNotYetValidException:
                errorCode = ErrorMessageCodes.TokenNotYetValid;
                logLevel = LogLevel.Information; // Log as info
                return true;
            case SecurityTokenInvalidSignatureException:
                errorCode = ErrorMessageCodes.TokenSignatureInvalid;
                logLevel = LogLevel.Information; // Log as info
                return true;
            case SecurityTokenException:
                errorCode = ErrorMessageCodes.TokenValidationFailed;
                logLevel = LogLevel.Information; // Log as info
                return true;
            default:
                errorCode = ErrorMessageCodes.TokenUnexpectedError;
                logLevel = LogLevel.Warning; // Unexpected exceptions should be warnings
                return true;
        }
    }
    
    private void LogException(Exception exception, LogLevel logLevel, string errorCode)
    {
        var message = $"Token validation failed with error: {errorCode}";

        switch (logLevel)
        {
            case LogLevel.Information:
            case LogLevel.Trace:
            case LogLevel.Debug:
                _logger.LogInformation(exception, message);
                break;
            case LogLevel.Warning:
            case LogLevel.Error:
            case LogLevel.Critical:
            case LogLevel.None:
            default:
                _logger.LogWarning(exception, message);
                break;
        }
    }
    
    #endregion
}