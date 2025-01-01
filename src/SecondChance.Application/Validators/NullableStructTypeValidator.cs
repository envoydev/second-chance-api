using FluentValidation;

namespace SecondChance.Application.Validators;

internal class NullableStructTypeValidator<T> : AbstractValidator<T?> where T : struct
{
    public NullableStructTypeValidator(AbstractValidator<T> innerValidator)
    {
        RuleFor(t => t)
            .Must(t => t.HasValue)
            .DependentRules(() =>
            {
                RuleFor(t => t!.Value).SetValidator(innerValidator);
            });
    }
}