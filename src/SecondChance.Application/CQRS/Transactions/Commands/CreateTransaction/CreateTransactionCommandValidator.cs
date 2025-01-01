using FluentValidation;
using SecondChance.Application.CQRS.Transactions.Validators;
using SecondChance.Application.Persistant;
using SecondChance.Application.Validators;

namespace SecondChance.Application.CQRS.Transactions.Commands.CreateTransaction;

// ReSharper disable once UnusedType.Global
internal sealed class CreateTransactionCommandValidator : AbstractValidator<CreateTransactionCommand>
{
    public CreateTransactionCommandValidator(IApplicationDbContext applicationDbContext)
    {
        RuleFor(v => v.ProjectId)
            .SetValidator(new TransactionProjectIdValidator(applicationDbContext));

        RuleFor(v => v.OperationType)
            .SetValidator(new TransactionOperationTypeValidator());

        RuleFor(v => v.Amount)
            .SetValidator(new TransactionAmountValidator());

        RuleFor(v => v.Description)
            .SetValidator(new NullValueValidator<string>(new TransactionDescriptionValidator()))
            .When(x => x.Description != null);
        
        RuleFor(v => v)
            .SetValidator(new TransactionCurrencyValidator());
    }
}