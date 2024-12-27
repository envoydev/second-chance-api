using FluentValidation;
using SecondChance.Application.Errors;
using SecondChance.Application.Models.Filters;

namespace SecondChance.Application.Validators;

public class PagerFilterValidator : AbstractValidator<IPagerFilter>
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