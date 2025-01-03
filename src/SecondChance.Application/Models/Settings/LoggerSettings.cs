using Microsoft.Extensions.Logging;

namespace SecondChance.Application.Models.Settings;

public class LoggerSettings
{
    public LogLevel ConsoleLogLevel { get; init; }
    public string ConsoleMessageTemplate { get; init; }
    public LogLevel FileLogLevel { get; init; }
    public string FileName { get; init; }
    public string FileFolderPath { get; init; }
}