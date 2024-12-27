using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Carter;
using Microsoft.AspNetCore.Mvc;
using SecondChance.Api.Presentation.Extensions;
using SecondChance.Api.Presentation.Logger.Enrichers;
using SecondChance.Api.Presentation.Services;
using SecondChance.Application.Services;
using Serilog.Core;

namespace SecondChance.Api.Presentation;

internal static class DependencyInjection
{
    public static void AddPresentation(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();

        services.AddApiVersioning(SetApiVersioningOptions)
                .AddApiExplorer(SetApiExplorerOptions);

        services.AddHttpContextAccessor();
        services.AddDataProtection();
        services.AddCors();
        services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);
        services.AddOpenApi();

        services.AddCarter(configurator: c =>
        {
            c.WithEmptyValidators();
            c.WithEmptyResponseNegotiators();
        });

        services.ConfigureHttpJsonOptions(options => { options.SerializerOptions.SetApplicationJsonSerializeRules(); });

        // Logger
        services.AddSingleton<ILogEventEnricher, HttpContextEnricher>();

        // Services
        services.AddScoped<ISessionService, SessionService>();
        services.AddSingleton<ISettingsService, SettingsService>();
        services.AddSingleton<IJsonSerializerService, JsonSerializeService>();
    }

    #region Private methods

    private static void SetApiVersioningOptions(ApiVersioningOptions options)
    {
        options.ReportApiVersions = true;
        options.DefaultApiVersion = new ApiVersion(1);
        options.AssumeDefaultVersionWhenUnspecified = true;
    }

    private static void SetApiExplorerOptions(ApiExplorerOptions options)
    {
        options.GroupNameFormat = "'v'VVV";
        options.SubstituteApiVersionInUrl = true;
    }

    #endregion
}