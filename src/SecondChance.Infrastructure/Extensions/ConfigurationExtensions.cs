using Ardalis.GuardClauses;
using Microsoft.Extensions.Configuration;
using SecondChance.Application.Models.Settings;

namespace SecondChance.Infrastructure.Extensions;

public static class ConfigurationExtensions
{
    internal static string GetDatabaseConnectionString(this IConfiguration configuration)
    {
        var applicationSettings = configuration.Get<ApplicationSettings>();

        Guard.Against.Null(applicationSettings, message: $"{nameof(ApplicationSettings)} cannot be null.");

        var connectionString = Guard.Against.NullOrWhiteSpace(applicationSettings.ConnectionStrings.DatabaseConnection,
            message: $"Connection string '{nameof(applicationSettings.ConnectionStrings.DatabaseConnection)}' not found.");

        return connectionString;
    }
}