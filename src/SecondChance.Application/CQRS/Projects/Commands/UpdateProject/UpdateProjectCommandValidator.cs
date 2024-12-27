using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SecondChance.Application.Errors;
using SecondChance.Application.Persistant;
using SecondChance.Domain.Validations;

namespace SecondChance.Application.CQRS.Projects.Commands.UpdateProject;

public class UpdateProjectCommandValidator : AbstractValidator<UpdateProjectCommand>
{
    private readonly IApplicationDbContext _applicationDbContext;

    public UpdateProjectCommandValidator(IApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;

        RuleFor(v => v.ProjectId)
            .NotEmpty()
            .WithErrorCode(ErrorMessageCodes.ValidationRequiredValue)
            .MustAsync(CheckIsProjectExistAsync)
            .WithErrorCode(ErrorMessageCodes.ValidationRecordNotFound);

        RuleFor(v => v.Name)
            .NotEmpty()
            .WithErrorCode(ErrorMessageCodes.ValidationRequiredValue)
            .Length(ProjectValidations.NameMinLength, ProjectValidations.NameMaxLength)
            .WithErrorCode(ErrorMessageCodes.ValidationInvalidRange)
            .WithState(_ => new { MinLength = ProjectValidations.NameMinLength, MaxLength = ProjectValidations.NameMaxLength })
            .MustAsync(CheckProjectWithSameIsNotExistAsync)
            .WithErrorCode(ErrorMessageCodes.ValidationSameValueExist);
    }

    private Task<bool> CheckIsProjectExistAsync(UpdateProjectCommand updateProjectCommand, Guid projectId, CancellationToken cancellationToken)
    {
        return _applicationDbContext.Projects.AsNoTracking().AnyAsync(x => x.Id == projectId, cancellationToken);
    }

    private async Task<bool> CheckProjectWithSameIsNotExistAsync(UpdateProjectCommand updateProjectCommand, string name, CancellationToken cancellationToken)
    {
        return !await _applicationDbContext.Projects.AsNoTracking()
                                           .Where(x => x.Id != updateProjectCommand.ProjectId)
                                           .AnyAsync(x => x.Name == name, cancellationToken);
    }
}