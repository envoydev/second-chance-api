using FluentValidation;
using SecondChance.Application.Errors;
using SecondChance.Domain.Validations;

namespace SecondChance.Application.CQRS.Users.Validators;

public class UserFirstNameValidator : AbstractValidator<string>
{
    public UserFirstNameValidator()
    {
        RuleFor(firstName => firstName)
            .Length(UserValidations.FirstNameMinLength, UserValidations.FirstNameMaxLength)
            .WithErrorCode(ErrorMessageCodes.ValidationInvalidRange)
            .WithState(_ => new { MinLength = UserValidations.FirstNameMinLength, MaxLength = UserValidations.LastNameMaxLength })
            .Matches(UserValidations.FirstNameCharactersValidation)
            .WithErrorCode(ErrorMessageCodes.ValidationInvalidCharacters)
            .WithState(_ => new { ValidCharacters = UserValidations.FirstNameAllowedCharacters });
    }
}