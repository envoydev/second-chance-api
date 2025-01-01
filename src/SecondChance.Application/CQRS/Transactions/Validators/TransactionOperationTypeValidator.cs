using FluentValidation;
using SecondChance.Application.Errors;
using SecondChance.Domain.Enums;

namespace SecondChance.Application.CQRS.Transactions.Validators;

internal class TransactionOperationTypeValidator : AbstractValidator<OperationType?>
{
    public TransactionOperationTypeValidator()
    {
        RuleFor(operationType => operationType)
            .NotEmpty()
            .WithErrorCode(ErrorMessageCodes.ValidationRequiredValue);
    }
}