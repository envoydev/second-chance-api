using FluentValidation;
using SecondChance.Application.Errors;
using SecondChance.Domain.Validations;

namespace SecondChance.Application.CQRS.Projects.Validators;

public class ProjectNameValidator : AbstractValidator<string>
{
    public ProjectNameValidator()
    {
        RuleFor(name => name)
            .NotEmpty()
            .WithErrorCode(ErrorMessageCodes.ValidationRequiredValue)
            .Length(ProjectValidations.NameMinLength, ProjectValidations.NameMaxLength)
            .WithErrorCode(ErrorMessageCodes.ValidationInvalidRange)
            .WithState(_ => new { MinLength = ProjectValidations.NameMinLength, MaxLength = ProjectValidations.NameMaxLength });
    }
}