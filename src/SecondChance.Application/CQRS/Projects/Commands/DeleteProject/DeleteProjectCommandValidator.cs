using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SecondChance.Application.Errors;
using SecondChance.Application.Persistant;

namespace SecondChance.Application.CQRS.Projects.Commands.DeleteProject;

public class DeleteProjectCommandValidator : AbstractValidator<DeleteProjectCommand>
{
    private readonly IApplicationDbContext _applicationDbContext;

    public DeleteProjectCommandValidator(IApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;

        RuleFor(v => v.ProjectId)
            .NotEmpty()
            .WithErrorCode(ErrorMessageCodes.ValidationRequiredValue)
            .MustAsync(CheckIsProjectExistAsync)
            .WithErrorCode(ErrorMessageCodes.ValidationRecordNotFound);
    }

    private Task<bool> CheckIsProjectExistAsync(DeleteProjectCommand deleteProjectCommand, Guid projectId, CancellationToken cancellationToken)
    {
        return _applicationDbContext.Projects.AsNoTracking().AnyAsync(x => x.Id == projectId, cancellationToken);
    }
}