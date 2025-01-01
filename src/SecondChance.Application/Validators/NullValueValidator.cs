using FluentValidation;

namespace SecondChance.Application.Validators;

internal class NullValueValidator<T> : AbstractValidator<T?>
{
    public NullValueValidator(AbstractValidator<T> innerValidator)
    {
        RuleFor(t => t)
            .Must(t => t != null)
            .DependentRules(() =>
            {
                RuleFor(t => t!).SetValidator(innerValidator);
            });
    }
}