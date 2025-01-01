using FluentValidation;
using SecondChance.Application.Errors;
using SecondChance.Domain.Validations;

namespace SecondChance.Application.CQRS.Transactions.Validators;

internal class TransactionDescriptionValidator : AbstractValidator<string>
{
    public TransactionDescriptionValidator()
    {
        RuleFor(description => description)
            .MaximumLength(TransactionValidations.DescriptionMaxLength)
            .WithErrorCode(ErrorMessageCodes.ValidationMaxLengthExceeded)
            .WithState(_ => new { MaxLength = TransactionValidations.DescriptionMaxLength });
    }
}