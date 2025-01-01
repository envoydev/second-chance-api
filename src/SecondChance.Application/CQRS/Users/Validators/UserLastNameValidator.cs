using FluentValidation;
using SecondChance.Application.Errors;
using SecondChance.Domain.Validations;

namespace SecondChance.Application.CQRS.Users.Validators;

public class UserLastNameValidator : AbstractValidator<string>
{
    public UserLastNameValidator()
    {
        RuleFor(lastName => lastName)
            .Length(UserValidations.LastNameMinLength, UserValidations.FirstNameMaxLength)
            .WithErrorCode(ErrorMessageCodes.ValidationInvalidRange)
            .WithState(_ => new { MinLength = UserValidations.LastNameMinLength, MaxLength = UserValidations.LastNameMaxLength })
            .Matches(UserValidations.LastNameCharactersValidation)
            .WithErrorCode(ErrorMessageCodes.ValidationInvalidCharacters)
            .WithState(_ => new { ValidCharacters = UserValidations.LastNameAllowedCharacters });
    }
}