namespace SecondChance.Application.Logger;

public interface IApplicationLogger<T>
{
    public void LogDebug(string? message, params object?[] args);
    public void LogInformation(string? message, params object?[] args);
    public void LogInformation(Exception? exception, string? message, params object?[] args);
    public void LogWarning(string? message, params object?[] args);
    public void LogWarning(Exception? exception, string? message, params object?[] args);
    public void LogError(string? message, params object?[] args);
    public void LogError(Exception? exception, string? message, params object?[] args);
}