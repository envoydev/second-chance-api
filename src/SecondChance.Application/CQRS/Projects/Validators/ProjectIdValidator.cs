using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SecondChance.Application.Errors;
using SecondChance.Application.Persistant;

namespace SecondChance.Application.CQRS.Projects.Validators;

public class ProjectIdValidator : AbstractValidator<Guid>
{
    private readonly IApplicationDbContext _applicationDbContext;
    
    public ProjectIdValidator(IApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;

        RuleFor(projectId => projectId)
            .NotEmpty()
            .WithErrorCode(ErrorMessageCodes.ValidationRequiredValue)
            .MustAsync(CheckIsTransactionExistAsync)
            .WithErrorCode(ErrorMessageCodes.ValidationRecordNotFound);
    }
    
    private Task<bool> CheckIsTransactionExistAsync(Guid projectIdObject, Guid projectId, CancellationToken cancellationToken)
    {
        return _applicationDbContext.Projects.AsNoTracking().AnyAsync(x => x.Id == projectId, cancellationToken);
    }
}