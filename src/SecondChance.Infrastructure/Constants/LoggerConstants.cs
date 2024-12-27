namespace SecondChance.Infrastructure.Constants;

public abstract class LoggerConstants
{
    public const string FilesFolder = "Errors";
    public const string FileName = "logs.txt";
    public const string ConsoleMessageTemplate = "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] [Thread:{ThreadId}/Task:{TaskId}] {SourceContext}: {Message:lj}{NewLine}{Exception}";
}