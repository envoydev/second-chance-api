using FluentValidation;
using SecondChance.Application.Errors;
using SecondChance.Application.Models.QueryFilters;

namespace SecondChance.Application.Validators;

internal class PagerFilterValidator : AbstractValidator<IPagerQueryFilter>
{
    public PagerFilterValidator()
    {
        RuleFor(v => v.Skip)
            .LessThan(0)
            .WithErrorCode(ErrorMessageCodes.ValidationInvalidValue)
            .When(x => x.Skip.HasValue);
        
        RuleFor(v => v.Take)
            .LessThan(0)
            .WithErrorCode(ErrorMessageCodes.ValidationInvalidValue)
            .When(x => x.Take.HasValue);
    }
}