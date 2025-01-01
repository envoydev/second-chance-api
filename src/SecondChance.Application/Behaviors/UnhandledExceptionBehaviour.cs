using MediatR;
using SecondChance.Application.Logger;
using SecondChance.Application.Services;

namespace SecondChance.Application.Behaviors;

internal sealed class UnhandledExceptionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    private readonly IApplicationLogger<TRequest> _logger;
    private readonly ISessionService _sessionService;

    public UnhandledExceptionBehaviour(IApplicationLogger<TRequest> logger,
        ISessionService sessionService)
    {
        _logger = logger;
        _sessionService = sessionService;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        try
        {
            return await next();
        }
        catch (Exception)
        {
            var userId = _sessionService.UserId?.ToString() ?? string.Empty;
            var userRole = _sessionService.UserRole?.ToString() ?? string.Empty;

            var requestName = typeof(TRequest).Name;

            _logger.LogError("Request: Unhandled Exception for Request {Name}. [UserId: {UserId} | UserRole: {UserRole}] {@Request}",
                requestName, userId, userRole, request);

            throw;
        }
    }
}