using FluentValidation;
using SecondChance.Application.Errors;
using SecondChance.Domain.Validations;

namespace SecondChance.Application.CQRS.Users.Validators;

internal class UserUserNameValidator : AbstractValidator<string>
{
    public UserUserNameValidator()
    {
        RuleFor(userName => userName)
            .NotEmpty()
            .WithErrorCode(ErrorMessageCodes.ValidationRequiredValue)
            .Length(UserValidations.EmailMinLength, UserValidations.EmailMaxLength)
            .WithErrorCode(ErrorMessageCodes.ValidationInvalidRange)
            .WithState(_ => new { MinLength = UserValidations.EmailMinLength, MaxLength = UserValidations.EmailMaxLength })
            .EmailAddress()
            .WithErrorCode(ErrorMessageCodes.ValidationInvalidCharacters);
    }
}