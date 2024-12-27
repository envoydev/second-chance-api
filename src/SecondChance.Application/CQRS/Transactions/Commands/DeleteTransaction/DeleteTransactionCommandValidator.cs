using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SecondChance.Application.Errors;
using SecondChance.Application.Persistant;

namespace SecondChance.Application.CQRS.Transactions.Commands.DeleteTransaction;

public class DeleteTransactionCommandValidator : AbstractValidator<DeleteTransactionCommand>
{
    private readonly IApplicationDbContext _applicationDbContext;

    public DeleteTransactionCommandValidator(IApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;

        RuleFor(v => v.TransactionId)
            .NotEmpty()
            .WithErrorCode(ErrorMessageCodes.ValidationRequiredValue)
            .MustAsync(CheckIsTransactionExistAsync)
            .WithErrorCode(ErrorMessageCodes.ValidationRecordNotFound);
    }

    private async Task<bool> CheckIsTransactionExistAsync(DeleteTransactionCommand deleteTransactionCommand, Guid transactionId, CancellationToken cancellationToken)
    {
        return await _applicationDbContext.Transactions.AsNoTracking().AnyAsync(x => x.Id == transactionId, cancellationToken);
    }
}