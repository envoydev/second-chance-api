using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SecondChance.Application.Errors;
using SecondChance.Application.Persistant;
using SecondChance.Domain.Validations;

namespace SecondChance.Application.CQRS.Transactions.Commands.CreateTransaction;

public class CreateTransactionCommandValidator : AbstractValidator<CreateTransactionCommand>
{
    private readonly IApplicationDbContext _applicationDbContext;
    
    public CreateTransactionCommandValidator(IApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;

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

    private async Task<bool> CheckIsProjectExistAsync(CreateTransactionCommand createTransactionCommand, Guid projectId, CancellationToken cancellationToken)
    {
        return await _applicationDbContext.Projects.AsNoTracking().AnyAsync(x => x.Id == projectId, cancellationToken);
    }
}