using FluentValidation.Results;

namespace SecondChance.Application.Errors.Exceptions;

public class BadRequestException : Exception
{
    private BadRequestException(string message) : base(message)
    {
        Errors = new Dictionary<string, string[]>();
    }

    public BadRequestException(string message, IEnumerable<ValidationFailure> failures) : this(message)
    {
        Errors = failures
                 .GroupBy(e => e.PropertyName, e => e.ErrorCode)
                 .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
    }

    public IDictionary<string, string[]> Errors { get; }
}