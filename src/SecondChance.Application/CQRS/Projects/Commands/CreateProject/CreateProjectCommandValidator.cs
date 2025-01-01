using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SecondChance.Application.CQRS.Projects.Validators;
using SecondChance.Application.Errors;
using SecondChance.Application.Persistant;

namespace SecondChance.Application.CQRS.Projects.Commands.CreateProject;

// ReSharper disable once UnusedType.Global
internal sealed class CreateProjectCommandValidator : AbstractValidator<CreateProjectCommand>
{
    private readonly IApplicationDbContext _applicationDbContext;

    public CreateProjectCommandValidator(IApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;

        RuleFor(v => v.Name)
            .SetValidator(new ProjectNameValidator())
            .MustAsync(CheckIsProjectWithSameNameDoesNotExistAsync)
            .WithErrorCode(ErrorMessageCodes.ValidationSameValueExist);
        
        RuleFor(v => v.Name)
            .SetValidator(new ProjectNameValidator())
            .MustAsync(CheckIsProjectWithSameNameDoesNotExistAsync)
            .WithErrorCode(ErrorMessageCodes.ValidationSameValueExist);
    }

    private Task<bool> CheckIsProjectWithSameNameDoesNotExistAsync(CreateProjectCommand createProjectCommand, 
        string name, CancellationToken cancellationToken)
    {
        return _applicationDbContext.Projects.AsNoTracking().AllAsync(x => x.Name != name, cancellationToken);
    }
}