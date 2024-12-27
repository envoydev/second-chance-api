using System.Reflection;
using MediatR;
using SecondChance.Application.Errors;
using SecondChance.Application.Errors.Exceptions;
using SecondChance.Application.Security;
using SecondChance.Application.Services;

namespace SecondChance.Application.Behaviors;

public class AuthorizationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    private readonly IDateTimeService _dateTimeService;
    private readonly ISessionService _sessionService;

    public AuthorizationBehaviour(ISessionService sessionService,
        IDateTimeService dateTimeService)
    {
        _sessionService = sessionService;
        _dateTimeService = dateTimeService;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var authorizeAttributes = request.GetType()
                                         .GetCustomAttributes<AuthorizeAttribute>()
                                         .ToList()
                                         .AsReadOnly();

        if (authorizeAttributes.Count == 0 || authorizeAttributes.All(x => x.Roles.Length != 0))
        {
            return await next();
        }

        if (!_sessionService.UserId.HasValue || !_sessionService.UserRole.HasValue || !_sessionService.TokenExpiration.HasValue)
        {
            throw new UnauthorizedException(ErrorMessageCodes.UnauthorizedAccess);
        }

        if (_sessionService.TokenExpiration < _dateTimeService.GetUtc())
        {
            throw new UnauthorizedException(ErrorMessageCodes.TokenExpired);
        }

        var isUserRolePresentInAttribute = authorizeAttributes.Where(x => x.Roles.Length != 0)
                                                              .Any(x => x.Roles.Contains(_sessionService.UserRole.Value));

        if (!isUserRolePresentInAttribute)
        {
            throw new ForbiddenException(ErrorMessageCodes.ForbiddenAccess);
        }

        return await next();
    }
}