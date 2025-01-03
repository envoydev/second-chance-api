using SecondChance.Application.Services;
using Serilog;
using Serilog.Core;
using Serilog.Extensions.Logging;
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
        var settings = _settingsService.GetSettings();

        Directory.CreateDirectory(settings.Logger.FileFolderPath);

        var logger = new LoggerConfiguration()
                     .Enrich.FromLogContext()
                     .Enrich.WithMachineName()
                     .Enrich.WithThreadId()
                     .Enrich.WithThreadName()
                     .Enrich.WithEnvironmentName()
                     .Enrich.With(_serilogEnrichers.ToArray())
                     .WriteTo.Console(LevelConvert.ToSerilogLevel(settings.Logger.ConsoleLogLevel),
                         outputTemplate: settings.Logger.ConsoleMessageTemplate,
                         theme: AnsiConsoleTheme.Code,
                         applyThemeToRedirectedOutput: true)
                     .WriteTo.File(_jsonTextFormatter,
                         Path.Combine(settings.Logger.FileFolderPath, settings.Logger.FileName),
                         LevelConvert.ToSerilogLevel(settings.Logger.FileLogLevel),
                         rollingInterval: RollingInterval.Day,
                         rollOnFileSizeLimit: true,
                         fileSizeLimitBytes: 25 * 1024 * 1024);

        return logger.CreateLogger();
    }
}