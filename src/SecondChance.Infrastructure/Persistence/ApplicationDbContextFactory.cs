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
        var rootPath = GetArgumentValue(args, "--path=", Directory.GetDirectoryRoot(Directory.GetCurrentDirectory()));
        Console.WriteLine($"Root path: {rootPath}");

        var environmentRuntime = GetEnumArgumentValue<EnvironmentRuntime>(args, "--environment=", EnvironmentObject.GetEnvironmentValue());
        Console.WriteLine($"Environment runtime: {environmentRuntime}");

        var configurationBuilder = new ConfigurationBuilder()
            .AddJsonFile(
                GetJsonFilePath(rootPath, $"appsettings.{environmentRuntime}.json"), 
                optional: true, 
                reloadOnChange: true);

        if (GetEnumArgumentValue<SecretsRuntime>(args, "--secrets=", EnvironmentObject.GetSecretsValue()) == SecretsRuntime.UseSecrets)
        {
            configurationBuilder.AddJsonFile(
                GetJsonFilePath(rootPath, "Root/Secrets", $"secrets.{environmentRuntime}.json"), 
                optional: true, 
                reloadOnChange: true);
        }

        var connectionString = configurationBuilder.Build().GetDatabaseConnectionString();
        Guard.Against.NullOrWhiteSpace(connectionString, "Connection string cannot be null or empty.");

        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilder.SetDatabaseType(connectionString);
        
        return new ApplicationDbContext(optionsBuilder.Options);
    }

    #region Private methods

    private static TEnum GetEnumArgumentValue<TEnum>(string[] args, string prefix, string? defaultValue) where TEnum : struct
    {
        var value = GetArgumentValue(args, prefix, defaultValue);
        return Enum.Parse<TEnum>(value);
    }
    
    private static string GetArgumentValue(string[] args, string prefix, string? defaultValue)
    {
        var value = args.FirstOrDefault(x => x.StartsWith(prefix))?.Replace(prefix, string.Empty) ?? defaultValue;
        Guard.Against.NullOrWhiteSpace(value, $"{prefix.Trim('=')} value is invalid.");
        return value;
    }

    private static string GetJsonFilePath(string rootPath, string folder, string fileName)
    {
        var path = Path.Combine(rootPath, folder, fileName);
        Console.WriteLine($"JSON file path: {path}");
        Guard.Against.Expression(result => result, !File.Exists(path), $"File not found at path: {path}");
        return path;
    }

    private static string GetJsonFilePath(string rootPath, string fileName)
    {
        return GetJsonFilePath(rootPath, string.Empty, fileName);
    }

    #endregion
}