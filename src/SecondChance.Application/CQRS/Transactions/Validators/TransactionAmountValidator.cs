using FluentValidation;
using SecondChance.Application.Errors;

namespace SecondChance.Application.CQRS.Transactions.Validators;

internal class TransactionAmountValidator : AbstractValidator<decimal>
{
    public TransactionAmountValidator()
    {
        RuleFor(amount => amount)
            .NotEmpty()
            .WithErrorCode(ErrorMessageCodes.ValidationRequiredValue)
            .GreaterThanOrEqualTo(0)
            .WithErrorCode(ErrorMessageCodes.ValidationInvalidValue);
    }
}