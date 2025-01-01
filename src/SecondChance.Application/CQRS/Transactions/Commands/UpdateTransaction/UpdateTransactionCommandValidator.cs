using FluentValidation;
using SecondChance.Application.CQRS.Transactions.Validators;
using SecondChance.Application.Persistant;
using SecondChance.Application.Validators;

namespace SecondChance.Application.CQRS.Transactions.Commands.UpdateTransaction;

// ReSharper disable once UnusedType.Global
internal sealed class UpdateTransactionCommandValidator : AbstractValidator<UpdateTransactionCommand>
{
    public UpdateTransactionCommandValidator(IApplicationDbContext applicationDbContext)
    {
        RuleFor(v => v.TransactionId)
            .SetValidator(new TransactionIdValidator(applicationDbContext));

        RuleFor(v => v.ProjectId)
            .SetValidator(new TransactionProjectIdValidator(applicationDbContext));

        RuleFor(v => v.OperationType)
            .SetValidator(new TransactionOperationTypeValidator());

        RuleFor(v => v.Amount)
            .SetValidator(new TransactionAmountValidator());

        RuleFor(v => v.Description)
            .SetValidator(new NullValueValidator<string>(new TransactionDescriptionValidator()))
            .When(v => v.Description != null);
        
        RuleFor(v => v)
            .SetValidator(new TransactionCurrencyValidator());
    }
}