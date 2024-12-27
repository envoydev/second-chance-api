using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SecondChance.Application.Errors;
using SecondChance.Application.Persistant;
using SecondChance.Domain.Validations;

namespace SecondChance.Application.CQRS.Transactions.Commands.UpdateTransaction;

public class UpdateTransactionCommandValidator : AbstractValidator<UpdateTransactionCommand>
{
    private readonly IApplicationDbContext _applicationDbContext;

    public UpdateTransactionCommandValidator(IApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;

        RuleFor(v => v.TransactionId)
            .NotEmpty()
            .WithErrorCode(ErrorMessageCodes.ValidationRequiredValue)
            .MustAsync(CheckIsTransactionExistAsync)
            .WithErrorCode(ErrorMessageCodes.ValidationRecordNotFound);

        RuleFor(v => v.ProjectId)
            .NotEmpty()
            .WithErrorCode(ErrorMessageCodes.ValidationRequiredValue)
            .MustAsync(CheckIsProjectExistAsync)
            .WithErrorCode(ErrorMessageCodes.ValidationRecordNotFound);

        RuleFor(v => v.CurrencyType)
            .NotEmpty()
            .WithErrorCode(ErrorMessageCodes.ValidationRequiredValue);

        RuleFor(v => v.OperationType)
            .NotEmpty()
            .WithErrorCode(ErrorMessageCodes.ValidationRequiredValue);

        RuleFor(v => v.Amount)
            .NotEmpty()
            .WithErrorCode(ErrorMessageCodes.ValidationRequiredValue);

        RuleFor(v => v.Description)
            .MaximumLength(TransactionValidations.DescriptionMaxLength)
            .WithErrorCode(ErrorMessageCodes.ValidationMaxLengthExceeded)
            .WithState(_ => new { MaxLength = TransactionValidations.DescriptionMaxLength });
    }

    private Task<bool> CheckIsTransactionExistAsync(UpdateTransactionCommand updateTransactionCommand, Guid transactionId, CancellationToken cancellationToken)
    {
        return _applicationDbContext.Transactions.AsNoTracking().AnyAsync(x => x.Id == transactionId, cancellationToken);
    }

    private Task<bool> CheckIsProjectExistAsync(UpdateTransactionCommand updateTransactionCommand, Guid projectId, CancellationToken cancellationToken)
    {
        return _applicationDbContext.Projects.AsNoTracking().AnyAsync(x => x.Id == projectId, cancellationToken);
    }
}