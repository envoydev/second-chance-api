using System.Net;
using SecondChance.Application.Errors;

namespace SecondChance.Api.Presentation.Middlewares;

public class NotFoundEndpointMiddleware
{
    private readonly RequestDelegate _next;

    public NotFoundEndpointMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        // Process the request by moving to the next middleware
        await _next(context);

        // If no endpoint matched, handle the 404 here
        if (context.Response.StatusCode == (int)HttpStatusCode.NotFound && context.GetEndpoint() == null)
        {
            var responseContent = new 
            {
                StatusCode = (int)HttpStatusCode.NotFound,
                Message = ErrorMessageCodes.NotFound
            };

            // Set response headers and body
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsJsonAsync(responseContent);
        }
    }
}