using SecondChance.Application.Constants;
using SystemEnvironment = System.Environment;

namespace SecondChance.Application.Environment;

public static class EnvironmentObject
{
    public static EnvironmentRuntime GetCurrentEnvironmentRuntime()
    {
        var environmentValue = SystemEnvironment.GetEnvironmentVariable(EnvironmentConstants.EnvironmentName);
        
        return Enum.TryParse(environmentValue, true, out EnvironmentRuntime environmentRuntime) 
            ? environmentRuntime 
            : EnvironmentRuntime.Development;
    }
    
    public static SecretsRuntime GetSecretsEnvironmentValue()
    {
        var environmentValue = SystemEnvironment.GetEnvironmentVariable(EnvironmentConstants.SecretsEnvironmentName);
        
        return Enum.TryParse(environmentValue, true, out SecretsRuntime secretsRuntime) 
            ? secretsRuntime 
            : SecretsRuntime.NotUse;
    }
}