using Ardalis.GuardClauses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using SecondChance.Application.Environment;
using SecondChance.Infrastructure.Extensions;

namespace SecondChance.Infrastructure.Persistence;

// ReSharper disable once UnusedType.Global
internal class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var rootPath = args.Length != 0 ? args[0] : string.Empty;
        Console.WriteLine($"Root path from args: {rootPath}");
        Guard.Against.NullOrWhiteSpace(rootPath, message: "Root path is invalid.");
        
        var environmentRuntime = EnvironmentObject.GetCurrentEnvironmentRuntime();
        Console.WriteLine($"Environment runtime: {environmentRuntime}");

        var jsonSettingsPath = Path.Combine(rootPath, $"appsettings.{environmentRuntime}.json");
        Console.WriteLine($"App-Settings file path: {jsonSettingsPath}");

        var isJsonFileExist = File.Exists(jsonSettingsPath);
        Guard.Against.Expression(result => result == false, isJsonFileExist, "Path to app-settings file does not exist.");

        var configurationBuilder = new ConfigurationBuilder();
        configurationBuilder.AddJsonFile(jsonSettingsPath, true, true);

        var environmentSecrets = EnvironmentObject.GetSecretsEnvironmentValue();
        Console.WriteLine($"Environment secrets: {environmentSecrets}");
        
        if (environmentSecrets == SecretsRuntime.UseSecrets)
        {
            var secretsJsonPath = Path.Combine(rootPath, "Root", "Secrets", $"secrets.{environmentRuntime}.json");
            Console.WriteLine($"Secrets file path: {secretsJsonPath}");
            Guard.Against.NullOrWhiteSpace(secretsJsonPath, message: "Secrets file path is invalid.");
            
            configurationBuilder.AddJsonFile(secretsJsonPath, true, true);
        }
        
        var connectionString = configurationBuilder.Build().GetDatabaseConnectionString();
        Console.WriteLine($"Connection string: {connectionString}");
        Guard.Against.NullOrWhiteSpace(connectionString, message: "Root project folder does not exist.");

        var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
        builder.SetDatabaseType(connectionString);

        return new ApplicationDbContext(builder.Options);
    }
}