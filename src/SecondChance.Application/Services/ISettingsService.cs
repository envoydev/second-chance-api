using SecondChance.Application.Environment;
using SecondChance.Application.Models.Settings;

namespace SecondChance.Application.Services;

public interface ISettingsService
{
    ApplicationSettings GetSettings();
    EnvironmentRuntime GetCurrentRuntimeEnvironment();
    string GetStaticFilesPath();
}