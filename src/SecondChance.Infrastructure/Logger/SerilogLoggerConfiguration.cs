using SecondChance.Application.Environment;
using SecondChance.Application.Services;
using SecondChance.Infrastructure.Constants;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting;
using Serilog.Sinks.SystemConsole.Themes;

namespace SecondChance.Infrastructure.Logger;

internal class SerilogLoggerConfiguration
{
    private readonly ITextFormatter _jsonTextFormatter;
    private readonly IEnumerable<ILogEventEnricher> _serilogEnrichers;
    private readonly ISettingsService _settingsService;

    public SerilogLoggerConfiguration(ISettingsService settingsService,
        ITextFormatter jsonTextFormatter,
        IEnumerable<ILogEventEnricher> serilogEnrichers)
    {
        _settingsService = settingsService;
        _jsonTextFormatter = jsonTextFormatter;
        _serilogEnrichers = serilogEnrichers;
    }

    public ILogger CreateConfiguration()
    {
        var minimumLogLevel = GetDefaultLogLevel(_settingsService.GetCurrentRuntimeEnvironment());
        var path = Path.Combine(_settingsService.GetStaticFilesPath(), LoggerConstants.FilesFolder);

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        var logger = new LoggerConfiguration()
                     .MinimumLevel.Is(minimumLogLevel)
                     .Enrich.FromLogContext()
                     .Enrich.WithMachineName()
                     .Enrich.WithThreadId()
                     .Enrich.WithThreadName()
                     .Enrich.WithEnvironmentName()
                     .Enrich.With(_serilogEnrichers.ToArray())
                     .WriteTo.Console(outputTemplate: LoggerConstants.ConsoleMessageTemplate,
                         theme: AnsiConsoleTheme.Code,
                         applyThemeToRedirectedOutput: true)
                     .WriteTo.File(_jsonTextFormatter,
                         Path.Combine(path, LoggerConstants.FileName),
                         LogEventLevel.Error,
                         rollingInterval: RollingInterval.Day,
                         rollOnFileSizeLimit: true,
                         fileSizeLimitBytes: 25 * 1024 * 1024);

        return logger.CreateLogger();
    }

    private static LogEventLevel GetDefaultLogLevel(EnvironmentRuntime environmentRuntime)
    {
        return environmentRuntime switch
        {
            EnvironmentRuntime.Development => LogEventLevel.Debug,
            EnvironmentRuntime.Production => LogEventLevel.Information,
            _ => LogEventLevel.Debug
        };
    }
}