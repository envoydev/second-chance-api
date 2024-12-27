using SecondChance.Application.Constants;
using SystemEnvironment = System.Environment;

namespace SecondChance.Application.Environment;

public static class EnvironmentObject
{
    public static string? GetEnvironmentValue()
    {
        return SystemEnvironment.GetEnvironmentVariable(EnvironmentConstants.EnvironmentName);
    }
    
    public static EnvironmentRuntime GetEnvironmentRuntime()
    {
        var environmentValue = GetEnvironmentValue();
        
        return Enum.TryParse(environmentValue, true, out EnvironmentRuntime environmentRuntime) 
            ? environmentRuntime 
            : EnvironmentRuntime.Development;
    }
    
    public static string? GetSecretsValue()
    {
        return SystemEnvironment.GetEnvironmentVariable(EnvironmentConstants.SecretsEnvironmentName);
    }
    
    public static SecretsRuntime GetSecretsRuntime()
    {
        var environmentValue = GetSecretsValue();
        
        return Enum.TryParse(environmentValue, true, out SecretsRuntime secretsRuntime) 
            ? secretsRuntime 
            : SecretsRuntime.NotUse;
    }
}