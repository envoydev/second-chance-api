using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SecondChance.Application.Errors;
using SecondChance.Application.Persistant;

namespace SecondChance.Application.CQRS.Transactions.Validators;

internal class TransactionIdValidator : AbstractValidator<Guid>
{
    private readonly IApplicationDbContext _applicationDbContext;
    
    public TransactionIdValidator(IApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;

        RuleFor(transactionId => transactionId)
            .NotEmpty()
            .WithErrorCode(ErrorMessageCodes.ValidationRequiredValue)
            .MustAsync(CheckIsTransactionExistAsync)
            .WithErrorCode(ErrorMessageCodes.ValidationRecordNotFound);
    }
    
    private Task<bool> CheckIsTransactionExistAsync(Guid transactionIdObject, Guid transactionId, CancellationToken cancellationToken)
    {
        return _applicationDbContext.Transactions.AsNoTracking().AnyAsync(x => x.Id == transactionId, cancellationToken);
    }
}