using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using SecondChance.Api.Presentation.Constants;
using SecondChance.Application.Constants;
using SecondChance.Application.Errors;
using SecondChance.Application.Errors.Exceptions;
using SecondChance.Application.Logger;
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
        var isEndpointHaveAuthorizationAttribute = context.GetEndpoint()?.Metadata.GetMetadata<AuthorizeAttribute>() != null;

        if (isEndpointHaveAuthorizationAttribute && string.IsNullOrWhiteSpace(token))
        {
            throw new UnauthorizedException(ErrorMessageCodes.UnauthorizedAccess);
        }

        if (string.IsNullOrWhiteSpace(token))
        {
            await _next(context);

            return;
        }

        var parsedToken = tokenService.ParseAccessToken(token);

        if (parsedToken == null)
        {
            await _next(context);

            return;
        }

        if (sessionService.UserId.HasValue && sessionService.UserId.Value != parsedToken.UserId)
        {
            logger.LogWarning("Wrong UserId from token. Session UserId: {SUserId}. Token UserId: {TUserId}",
                sessionService.UserId, parsedToken.UserId);

            await _next(context);

            return;
        }

        if (sessionService.UserRole.HasValue && sessionService.UserRole.Value != parsedToken.Role)
        {
            logger.LogWarning("Wrong Role from token. Session UserId: {SRole}. Token UserId: {TRole}",
                sessionService.UserRole, parsedToken.Role);

            await _next(context);

            return;
        }

        var claims = new[]
        {
            new Claim(TokenConstants.UserId, parsedToken.UserId.ToString()),
            new Claim(TokenConstants.Role, parsedToken.Role.ToString()),
            new Claim(TokenConstants.IssuedAt, parsedToken.IssuedAtUnixTimeStamp.ToString()),
            new Claim(TokenConstants.ExpiredAt, parsedToken.ExpiredAtUnixTimeStamp.ToString())
        };

        context.User = new ClaimsPrincipal(new ClaimsIdentity(claims, HttpItemsConstants.AuthenticationScheme));

        await _next(context);
    }
}