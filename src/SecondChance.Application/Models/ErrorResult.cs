namespace SecondChance.Application.Models;

public record ErrorResult(int StatusCode, string? Details = null, IDictionary<string, string[]>? Errors = null);