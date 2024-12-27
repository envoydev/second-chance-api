using Microsoft.Extensions.Logging;
using SecondChance.Application.Logger;

namespace SecondChance.Infrastructure.Logger;

internal class ApplicationLogger<T> : IApplicationLogger<T>
{
    private readonly ILogger<T> _logger;

    public ApplicationLogger(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<T>();
    }

    public void LogDebug(string? message, params object?[] args)
    {
        #pragma warning disable CA2254
        _logger.LogDebug(message, args);
        #pragma warning restore CA2254
    }

    public void LogInformation(string? message, params object?[] args)
    {
        #pragma warning disable CA2254
        _logger.LogInformation(message, args);
        #pragma warning restore CA2254
    }

    public void LogWarning(string? message, params object?[] args)
    {
        #pragma warning disable CA2254
        _logger.LogWarning(message, args);
        #pragma warning restore CA2254
    }

    public void LogWarning(Exception? exception, string? message, params object?[] args)
    {
        #pragma warning disable CA2254
        _logger.LogWarning(exception, message, args);
        #pragma warning restore CA2254
    }

    public void LogError(string? message, params object?[] args)
    {
        #pragma warning disable CA2254
        _logger.LogError(message, args);
        #pragma warning restore CA2254
    }

    public void LogError(Exception? exception, string? message, params object?[] args)
    {
        #pragma warning disable CA2254
        _logger.LogError(exception, message, args);
        #pragma warning restore CA2254
    }
}