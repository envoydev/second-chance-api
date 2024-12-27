using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SecondChance.Application.Errors;
using SecondChance.Application.Persistant;
using SecondChance.Domain.Validations;

namespace SecondChance.Application.CQRS.Projects.Commands.CreateProject;

public class CreateProjectCommandValidator : AbstractValidator<CreateProjectCommand>
{
    private readonly IApplicationDbContext _applicationDbContext;

    public CreateProjectCommandValidator(IApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;

        RuleFor(v => v.Name)
            .NotEmpty()
            .WithErrorCode(ErrorMessageCodes.ValidationRequiredValue)
            .Length(ProjectValidations.NameMinLength, ProjectValidations.NameMaxLength)
            .WithErrorCode(ErrorMessageCodes.ValidationInvalidRange)
            .WithState(_ => new { MinLength = ProjectValidations.NameMinLength, MaxLength = ProjectValidations.NameMaxLength })
            .MustAsync(CheckIsProjectWithSameNameDoesNotExistAsync)
            .WithErrorCode(ErrorMessageCodes.ValidationSameValueExist);
    }

    private Task<bool> CheckIsProjectWithSameNameDoesNotExistAsync(CreateProjectCommand createProjectCommand, 
        string name, CancellationToken cancellationToken)
    {
        return _applicationDbContext.Projects.AsNoTracking().AllAsync(x => x.Name != name, cancellationToken);
    }
}