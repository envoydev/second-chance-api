using System.Collections.ObjectModel;
using System.Security.Claims;
using SecondChance.Api.Presentation.Constants;
using SecondChance.Application.Constants;
using SecondChance.Application.Errors;
using SecondChance.Application.Errors.Exceptions;
using SecondChance.Application.Logger;
using SecondChance.Application.Security;
using SecondChance.Application.Services;

namespace SecondChance.Api.Presentation.Middlewares;

public class JwtBearerMiddleware
{
    private readonly RequestDelegate _next;

    public JwtBearerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, IApplicationLogger<JwtBearerMiddleware> logger, ITokenService tokenService, ISessionService sessionService)
    {
        var token = context.Request.Headers.Authorization.FirstOrDefault()?.Replace($"{HttpItemsConstants.AuthenticationScheme} ", string.Empty);
        var authorizationAttributes = context.GetEndpoint()?.Metadata.GetOrderedMetadata<AuthorizationAttribute>() 
                                  ?? ReadOnlyCollection<AuthorizationAttribute>.Empty;

        if (authorizationAttributes.Count > 0 && string.IsNullOrWhiteSpace(token))
        {
            throw new UnauthorizedException(ErrorMessageCodes.UnauthorizedAccess);
        }
        
        if (string.IsNullOrWhiteSpace(token))
        {
            await _next(context);

            return;
        }

        var parsedToken = tokenService.ParseAccessToken(token);
        if (!parsedToken.IsValid)
        {
            throw new UnauthorizedException(parsedToken.ErrorCode!);
        }
        
        if (sessionService.UserId.HasValue && sessionService.UserId.Value != parsedToken.Token!.UserId)
        {
            logger.LogWarning("Wrong UserId from token. Session UserId: {SUserId}. Token UserId: {TUserId}",
                sessionService.UserId, parsedToken.Token.UserId);

            throw new UnauthorizedException(ErrorMessageCodes.TokenInvalid);
        }

        if (sessionService.UserRole.HasValue && sessionService.UserRole.Value != parsedToken.Token!.Role)
        {
            logger.LogWarning("Wrong Role from token. Session UserId: {SRole}. Token UserId: {TRole}",
                sessionService.UserRole, parsedToken.Token.Role);

            throw new UnauthorizedException(ErrorMessageCodes.TokenInvalid);
        }

        var isUserRolePresentInAttribute = authorizationAttributes.Where(x => x.Roles.Length != 0)
                                                                  .Any(x => x.Roles.Contains(parsedToken.Token!.Role));

        if (!isUserRolePresentInAttribute)
        {
            throw new ForbiddenException(ErrorMessageCodes.ForbiddenAccess);
        }
        
        var claims = new[]
        {
            new Claim(TokenConstants.UserId, parsedToken.Token!.UserId.ToString()),
            new Claim(TokenConstants.Role, parsedToken.Token!.Role.ToString()),
            new Claim(TokenConstants.IssuedAt, parsedToken.Token!.IssuedAtUnixTimeStamp.ToString()),
            new Claim(TokenConstants.ExpiredAt, parsedToken.Token!.ExpiredAtUnixTimeStamp.ToString())
        };

        context.User = new ClaimsPrincipal(new ClaimsIdentity(claims, HttpItemsConstants.AuthenticationScheme));

        await _next(context);
    }
}