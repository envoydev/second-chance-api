using Ardalis.GuardClauses;
using SecondChance.Api.Presentation.Constants;
using SecondChance.Application.Environment;
using SecondChance.Application.Models.Settings;
using SecondChance.Application.Services;

namespace SecondChance.Api.Presentation.Services;

public class SettingsService : ISettingsService
{
    private readonly IConfiguration _configuration;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private ApplicationSettings? _applicationSettings;

    public SettingsService(IConfiguration configuration,
        IWebHostEnvironment webHostEnvironment)
    {
        _configuration = configuration;
        _webHostEnvironment = webHostEnvironment;
    }

    public ApplicationSettings GetSettings()
    {
        _applicationSettings ??= _configuration.Get<ApplicationSettings>();

        return Guard.Against.Null(_applicationSettings, message: $"{nameof(ApplicationSettings)} cannot be null!");
    }

    public EnvironmentRuntime GetCurrentRuntimeEnvironment()
    {
        var parsedEnum = Enum.TryParse(_webHostEnvironment.EnvironmentName, out EnvironmentRuntime environmentRuntime);

        Guard.Against.Expression(result => result == false, parsedEnum,
            $"{nameof(EnvironmentRuntime)} does not contain {nameof(_webHostEnvironment.EnvironmentName)} value");

        return environmentRuntime;
    }

    public string GetStaticFilesPath()
    {
        Guard.Against.NullOrWhiteSpace(_webHostEnvironment.ContentRootPath,
            message: $"{nameof(_webHostEnvironment.ContentRootPath)} cannot be null or empty!");

        var staticFilesPath = Path.Combine(_webHostEnvironment.ContentRootPath, PresentationConstants.StaticFilesFolderName);

        return staticFilesPath;
    }
}