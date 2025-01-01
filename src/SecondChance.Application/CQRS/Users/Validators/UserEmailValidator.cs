using FluentValidation;
using SecondChance.Application.Errors;
using SecondChance.Domain.Validations;

namespace SecondChance.Application.CQRS.Users.Validators;

internal class UserEmailValidator : AbstractValidator<string>
{
    public UserEmailValidator()
    {
        RuleFor(email => email)
            .NotEmpty()
            .WithErrorCode(ErrorMessageCodes.ValidationRequiredValue)
            .Length(UserValidations.UserNameMinLength, UserValidations.UserNameMaxLength)
            .WithErrorCode(ErrorMessageCodes.ValidationInvalidRange)
            .WithState(_ => new { MinLength = UserValidations.UserNameMinLength, MaxLength = UserValidations.UserNameMaxLength })
            .Matches(UserValidations.UserNameCharactersValidation)
            .WithErrorCode(ErrorMessageCodes.ValidationInvalidCharacters)
            .WithState(_ => new { ValidCharacters = UserValidations.UserNameAllowedCharacters });
    }
}