using System.Net;
using System.Text;
using SecondChance.Api.Presentation.Constants;
using SecondChance.Application.Errors;
using SecondChance.Application.Errors.Exceptions;
using SecondChance.Application.Logger;
using SecondChance.Application.Models;
using SecondChance.Application.Services;

namespace SecondChance.Api.Presentation.Middlewares;

public class GlobalExceptionMiddleware
{
    private readonly Dictionary<Type, Func<HttpContext, Exception, CancellationToken, Task>> _exceptionHandlers;
    private readonly IJsonSerializerService _jsonSerializerService;
    private readonly RequestDelegate _next;

    public GlobalExceptionMiddleware(RequestDelegate next,
        IJsonSerializerService jsonSerializerService)
    {
        _next = next;
        _jsonSerializerService = jsonSerializerService;

        _exceptionHandlers = GetExceptionHandlers();
    }

    // ReSharper disable once UnusedMember.Global
    public async Task Invoke(HttpContext httpContext, IApplicationLogger<GlobalExceptionMiddleware> logger)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Middleware caught an error. Exception message: {Message}", exception.Message);

            var exceptionType = exception.GetType();

            if (_exceptionHandlers.TryGetValue(exceptionType, out var handler))
            {
                await handler.Invoke(httpContext, exception, CancellationToken.None);
            }
            else
            {
                var requestError = new ErrorResult((int)HttpStatusCode.InternalServerError, ErrorMessageCodes.InternalServerError);

                await WriteResponseAsync(httpContext, requestError, CancellationToken.None);
            }
        }
    }

    private Dictionary<Type, Func<HttpContext, Exception, CancellationToken, Task>> GetExceptionHandlers()
    {
        return new Dictionary<Type, Func<HttpContext, Exception, CancellationToken, Task>>
        {
            { typeof(BadRequestException), HandleValidationException },
            { typeof(NotFoundException), HandleNotFoundException },
            { typeof(UnauthorizedException), HandleUnauthorizedAccessException },
            { typeof(ForbiddenException), HandleForbiddenAccessException }
        };
    }

    private async Task HandleValidationException(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var badRequestException = (BadRequestException)exception;

        var requestError = new ErrorResult((int)HttpStatusCode.BadRequest, badRequestException.Message, badRequestException.Errors);

        await WriteResponseAsync(httpContext, requestError, cancellationToken);
    }

    private async Task HandleNotFoundException(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var notFoundException = (NotFoundException)exception;

        var requestError = new ErrorResult((int)HttpStatusCode.NotFound, notFoundException.Message);

        await WriteResponseAsync(httpContext, requestError, cancellationToken);
    }

    private async Task HandleUnauthorizedAccessException(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var unAuthorizedException = (UnauthorizedException)exception;

        var requestError = new ErrorResult((int)HttpStatusCode.Unauthorized, unAuthorizedException.Message);

        await WriteResponseAsync(httpContext, requestError, cancellationToken);
    }

    private async Task HandleForbiddenAccessException(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var forbiddenException = (ForbiddenException)exception;

        var requestError = new ErrorResult((int)HttpStatusCode.Forbidden, forbiddenException.Message);

        await WriteResponseAsync(httpContext, requestError, cancellationToken);
    }

    private async Task WriteResponseAsync(HttpContext httpContext, ErrorResult errorResult, CancellationToken cancellationToken)
    {
        var json = _jsonSerializerService.SerializeObject(errorResult);

        using var body = new MemoryStream();

        var buffer = Encoding.UTF8.GetBytes(json);
        body.Write(buffer, 0, buffer.Length);
        body.Position = 0;

        httpContext.Response.Headers.Append(HttpItemsConstants.ContentTypeHeader, HttpItemsConstants.ContentTypeValue);
        httpContext.Response.StatusCode = errorResult.StatusCode;

        await body.CopyToAsync(httpContext.Response.Body, cancellationToken);
    }
}