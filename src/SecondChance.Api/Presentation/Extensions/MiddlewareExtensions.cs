using SecondChance.Api.Presentation.Middlewares;
using SecondChance.Application.Persistant;

namespace SecondChance.Api.Presentation.Extensions;

internal static class MiddlewareExtensions
{
    public static void UseGlobalExceptionHandler(this IApplicationBuilder builder)
    {
        builder.UseMiddleware<GlobalExceptionMiddleware>();
    }

    public static void UseJwtBearerAuthorization(this IApplicationBuilder builder)
    {
        builder.UseMiddleware<JwtBearerMiddleware>();
    }
    
    public static void UseNotFoundEndpointHandler(this IApplicationBuilder builder)
    {
        builder.UseMiddleware<NotFoundEndpointMiddleware>();
    }
    
    public static async Task InitializeDatabaseHandler(this IApplicationBuilder builder)
    {
        using var scope = builder.ApplicationServices.CreateScope();
        var initializer = scope.ServiceProvider.GetRequiredService<IApplicationDbContextInitializer>();
        await initializer.InitialiseAsync();
        await initializer.SeedAsync();
    }
}