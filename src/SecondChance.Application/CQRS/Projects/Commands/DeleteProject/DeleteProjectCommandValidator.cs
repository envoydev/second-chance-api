using FluentValidation;
using SecondChance.Application.CQRS.Projects.Validators;
using SecondChance.Application.Persistant;

namespace SecondChance.Application.CQRS.Projects.Commands.DeleteProject;

// ReSharper disable once UnusedType.Global
internal sealed class DeleteProjectCommandValidator : AbstractValidator<DeleteProjectCommand>
{
    public DeleteProjectCommandValidator(IApplicationDbContext applicationDbContext)
    {
        RuleFor(v => v.ProjectId)
            .SetValidator(new ProjectIdValidator(applicationDbContext));
    }
}