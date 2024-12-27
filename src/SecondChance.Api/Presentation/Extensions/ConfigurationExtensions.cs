using Ardalis.GuardClauses;
using SecondChance.Api.Presentation.Constants;
using SecondChance.Application.Environment;

namespace SecondChance.Api.Presentation.Extensions;

public static class ConfigurationExtensions
{
    public static void AddApplicationSettings(this IConfigurationBuilder configurationBuilder, IHostEnvironment environment)
    {
        configurationBuilder.AddJsonFile($"appsettings.{environment.EnvironmentName}.json", optional: true, reloadOnChange: true);

        var secretsRuntime = EnvironmentObject.GetSecretsRuntime();
        if (secretsRuntime == SecretsRuntime.UseSecrets)
        {
            var envSecretsPath = Path.Combine(environment.ContentRootPath, PresentationConstants.StaticFilesFolderName, 
                PresentationConstants.SecretsFolderName, $"secrets.{environment.EnvironmentName}.json");
            
            Guard.Against.NullOrWhiteSpace(envSecretsPath, message: "Secrets file is not found is secrets folder.");
            
            configurationBuilder.AddJsonFile(envSecretsPath, optional: true, reloadOnChange: true);   
        }
        
        configurationBuilder.AddEnvironmentVariables();
    }
}