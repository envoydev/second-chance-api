using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SecondChance.Application.CQRS.Projects.Validators;
using SecondChance.Application.Errors;
using SecondChance.Application.Persistant;

namespace SecondChance.Application.CQRS.Projects.Commands.UpdateProject;

// ReSharper disable once UnusedType.Global
internal sealed class UpdateProjectCommandValidator : AbstractValidator<UpdateProjectCommand>
{
    private readonly IApplicationDbContext _applicationDbContext;

    public UpdateProjectCommandValidator(IApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;

        RuleFor(v => v.ProjectId)
            .SetValidator(new ProjectIdValidator(applicationDbContext));
        
        RuleFor(v => v.Name)
            .SetValidator(new ProjectNameValidator())
            .MustAsync(CheckProjectWithSameIsNotExistAsync)
            .WithErrorCode(ErrorMessageCodes.ValidationSameValueExist);
    }

    private async Task<bool> CheckProjectWithSameIsNotExistAsync(UpdateProjectCommand updateProjectCommand, string name, CancellationToken cancellationToken)
    {
        return !await _applicationDbContext.Projects.AsNoTracking()
                                           .Where(x => x.Id != updateProjectCommand.ProjectId)
                                           .AnyAsync(x => x.Name == name, cancellationToken);
    }
}