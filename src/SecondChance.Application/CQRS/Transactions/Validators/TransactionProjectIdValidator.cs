using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SecondChance.Application.Errors;
using SecondChance.Application.Persistant;

namespace SecondChance.Application.CQRS.Transactions.Validators;

internal class TransactionProjectIdValidator : AbstractValidator<Guid>
{
    private readonly IApplicationDbContext _applicationDbContext;
    
    public TransactionProjectIdValidator(IApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;

        RuleFor(projectId => projectId)
            .NotEmpty()
            .WithErrorCode(ErrorMessageCodes.ValidationRequiredValue)
            .MustAsync(CheckIsProjectExistAsync)
            .WithErrorCode(ErrorMessageCodes.ValidationRecordNotFound);
    }
    
    private async Task<bool> CheckIsProjectExistAsync(Guid projectIdObject, Guid projectId, CancellationToken cancellationToken)
    {
        return await _applicationDbContext.Projects.AsNoTracking().AnyAsync(x => x.Id == projectId, cancellationToken);
    }
}