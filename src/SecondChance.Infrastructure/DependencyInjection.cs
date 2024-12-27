using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using SecondChance.Application.Environment;
using SecondChance.Application.Logger;
using SecondChance.Application.Models.Settings;
using SecondChance.Application.Persistant;
using SecondChance.Application.Services;
using SecondChance.Infrastructure.Extensions;
using SecondChance.Infrastructure.Logger;
using SecondChance.Infrastructure.Logger.Enrichers;
using SecondChance.Infrastructure.Logger.Formatters;
using SecondChance.Infrastructure.Persistence;
using SecondChance.Infrastructure.Persistence.Interceptors;
using SecondChance.Infrastructure.Services;
using Serilog.Core;
using Serilog.Formatting;

namespace SecondChance.Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services, ApplicationSettings applicationSettings)
    {
        // Database
        services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
        {
            options.AddInterceptors(serviceProvider.GetServices<ISaveChangesInterceptor>());
            options.SetDatabaseType(applicationSettings.ConnectionStrings.DatabaseConnection);

            var settingsService = serviceProvider.GetRequiredService<ISettingsService>();
            
            if (settingsService.GetCurrentRuntimeEnvironment() != EnvironmentRuntime.Development)
            {
                return;
            }

            options.EnableDetailedErrors();
            options.EnableSensitiveDataLogging();
        });
        services.AddScoped<IApplicationDbContext>(serviceProvider => serviceProvider.GetRequiredService<ApplicationDbContext>());
        services.AddTransient<IApplicationDbContextInitializer, ApplicationDbContextInitializer>();

        // Logger
        services.AddSingleton<ITextFormatter, SerilogJsonFormatter>();
        services.AddSingleton<ILogEventEnricher, TaskIdEnricher>();
        services.AddSingleton<SerilogLoggerConfiguration>();
        services.AddLogging(loggingBuilder => loggingBuilder.AddSerilogLogging());
        services.AddScoped(typeof(IApplicationLogger<>), typeof(ApplicationLogger<>));

        // Services
        services.AddScoped<IPasswordService, PasswordService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddSingleton<IDateTimeService, DateTimeService>();
    }
}