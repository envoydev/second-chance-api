using FluentValidation;
using SecondChance.Application.CQRS.Transactions.Validators;
using SecondChance.Application.Persistant;

namespace SecondChance.Application.CQRS.Transactions.Commands.DeleteTransaction;

// ReSharper disable once UnusedType.Global
internal sealed class DeleteTransactionCommandValidator : AbstractValidator<DeleteTransactionCommand>
{
    public DeleteTransactionCommandValidator(IApplicationDbContext applicationDbContext)
    {
        RuleFor(v => v.TransactionId)
            .SetValidator(new TransactionIdValidator(applicationDbContext));
    }
}