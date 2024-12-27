using FluentValidation;
using SecondChance.Application.Errors;
using SecondChance.Application.Models.Filters;

namespace SecondChance.Application.Validators;

public class DateRangeFilterValidator : AbstractValidator<IDateRangeFilter>
{
    public DateRangeFilterValidator()
    {
        RuleFor(v => v.From)
            .NotEmpty()
            .WithErrorCode(ErrorMessageCodes.ValidationInvalidValue)
            .MustAsync(CheckIsFromLowerThenToAsync)
            .WithErrorCode(ErrorMessageCodes.ValidationInvalidRange)
            .When(x => x.From.HasValue);
        
        RuleFor(v => v.To)
            .NotEmpty()
            .WithErrorCode(ErrorMessageCodes.ValidationInvalidValue)
            .MustAsync(CheckIsToHigherThenFromAsync)
            .WithErrorCode(ErrorMessageCodes.ValidationInvalidRange)
            .When(x => x.To.HasValue);
    }

    private static Task<bool> CheckIsFromLowerThenToAsync(IDateRangeFilter dateRangeFilter, DateTime? from, CancellationToken cancellationToken)
    {
        return !dateRangeFilter.To.HasValue ? Task.FromResult(true) : Task.FromResult(dateRangeFilter.To >= from);
    }
    
    private static Task<bool> CheckIsToHigherThenFromAsync(IDateRangeFilter dateRangeFilter, DateTime? to, CancellationToken cancellationToken)
    {
        return !dateRangeFilter.From.HasValue ? Task.FromResult(true) : Task.FromResult(dateRangeFilter.From <= to);
    }
}