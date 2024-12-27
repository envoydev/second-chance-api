using Serilog.Core;
using Serilog.Events;

namespace SecondChance.Api.Presentation.Logger.Enrichers;

internal class HttpContextEnricher : ILogEventEnricher
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HttpContextEnricher(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        var httpContext = _httpContextAccessor.HttpContext;

        if (httpContext == null)
        {
            return;
        }

        logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("RequestScheme", httpContext.Request.Scheme));
        logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("RequestHost", httpContext.Request.Host.Value));
        logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("RequestPath", httpContext.Request.Path));
        logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("RequestMethod", httpContext.Request.Method));
        logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("UserAgent", httpContext.Request.Headers.UserAgent));
        logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("RemoteIpAddress", httpContext.Connection.RemoteIpAddress));

        // Let us say we get CorrelationId passed in via request header, let us see how we can pull and populate that
        if (httpContext.Request.Headers.TryGetValue("App-Correlation-Id", out var appCorrelationId))
        {
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("CorrelationId", appCorrelationId));
        }
    }
}