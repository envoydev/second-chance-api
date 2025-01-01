using FluentValidation;
using SecondChance.Application.Errors;
using SecondChance.Application.Models.QueryFilters;

namespace SecondChance.Application.Validators;

internal class DateRangeQueryFilterValidator : AbstractValidator<IDateRangeQueryFilter>
{
    public DateRangeQueryFilterValidator()
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

    private static Task<bool> CheckIsFromLowerThenToAsync(IDateRangeQueryFilter dateRangeQueryFilter, DateTime? from, CancellationToken cancellationToken)
    {
        return !dateRangeQueryFilter.To.HasValue ? Task.FromResult(true) : Task.FromResult(dateRangeQueryFilter.To >= from);
    }
    
    private static Task<bool> CheckIsToHigherThenFromAsync(IDateRangeQueryFilter dateRangeQueryFilter, DateTime? to, CancellationToken cancellationToken)
    {
        return !dateRangeQueryFilter.From.HasValue ? Task.FromResult(true) : Task.FromResult(dateRangeQueryFilter.From <= to);
    }
}