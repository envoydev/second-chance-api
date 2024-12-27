using System.Diagnostics;
using MediatR;
using SecondChance.Application.Logger;
using SecondChance.Application.Services;

namespace SecondChance.Application.Behaviors;

public class PerformanceBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    private readonly IApplicationLogger<TRequest> _logger;
    private readonly ISessionService _sessionService;
    private readonly Stopwatch _timer;

    public PerformanceBehaviour(IApplicationLogger<TRequest> logger,
        ISessionService sessionService)
    {
        _logger = logger;
        _sessionService = sessionService;

        _timer = new Stopwatch();
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        _timer.Start();

        var response = await next();

        _timer.Stop();

        var elapsedMilliseconds = _timer.ElapsedMilliseconds;

        if (elapsedMilliseconds <= 500)
        {
            return response;
        }

        var requestName = typeof(TRequest).Name;
        var userId = _sessionService.UserId?.ToString() ?? string.Empty;
        var userRole = _sessionService.UserRole?.ToString() ?? string.Empty;

        _logger.LogWarning("Long Running Request: {Name} ({ElapsedMilliseconds} milliseconds) [{@UserId} | {@UserRole}] | {@Request}",
            requestName, elapsedMilliseconds, userId, userRole, request);

        return response;
    }
}