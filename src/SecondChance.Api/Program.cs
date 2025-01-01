using Carter;
using Scalar.AspNetCore;
using SecondChance.Api.Presentation;
using SecondChance.Api.Presentation.Extensions;
using SecondChance.Application;
using SecondChance.Application.Environment;
using SecondChance.Infrastructure;

try
{
    var builder = WebApplication.CreateBuilder(new WebApplicationOptions
    {
        EnvironmentName = EnvironmentObject.GetEnvironmentRuntime().ToString(),
        ContentRootPath = AppContext.BaseDirectory,
        Args = args
    });
    
    builder.Configuration.AddApplicationSettings(builder.Environment);
    
    builder.Services.AddApplication();
    builder.Services.AddInfrastructure(builder.Configuration.GetApplicationSettings());
    builder.Services.AddPresentation();

    var app = builder.Build();

    await app.InitializeDatabaseHandler();

    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
        app.MapScalarApiReference();
    }

    app.UseHttpsRedirection();
    app.UseGlobalExceptionHandler();

    if (app.Environment.IsProduction())
    {
        var applicationSettings = app.Configuration.GetApplicationSettings();
        app.UseCors(corsBuilder =>
        {
            corsBuilder.AllowAnyMethod()
                       .AllowAnyHeader()
                       .WithOrigins(applicationSettings.RequestConfig.Cors)
                       .AllowCredentials();
        });
    }

    app.UseJwtBearerAuthorization();
    app.UseNotFoundEndpointHandler();
    app.MapCarter();

    app.Run();

    Environment.Exit(0);
}
catch (Exception exception)
{
    var message = $"{exception.Message}{Environment.NewLine}{Environment.NewLine}{exception.StackTrace ?? string.Empty}";
    Console.WriteLine(message);

    Environment.Exit(-1);
}