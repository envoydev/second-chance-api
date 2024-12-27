using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SecondChance.Application.Errors;
using SecondChance.Application.Persistant;
using SecondChance.Application.Services;
using SecondChance.Domain.Enums;
using SecondChance.Domain.Validations;

namespace SecondChance.Application.CQRS.Users.Commands.CreateUser;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    private readonly IApplicationDbContext _applicationDbContext;
    private readonly ISessionService _sessionService;

    public CreateUserCommandValidator(IApplicationDbContext applicationDbContext, 
        ISessionService sessionService)
    {
        _applicationDbContext = applicationDbContext;
        _sessionService = sessionService;

        RuleFor(v => v.UserName)
            .NotEmpty()
            .WithErrorCode(ErrorMessageCodes.ValidationRequiredValue)
            .Length(UserValidations.UserNameMinLength, UserValidations.UserNameMaxLength)
            .WithErrorCode(ErrorMessageCodes.ValidationInvalidRange)
            .WithState(_ => new { MinLength = UserValidations.UserNameMinLength, MaxLength = UserValidations.UserNameMaxLength })
            .Matches(UserValidations.UserNameCharactersValidation)
            .WithErrorCode(ErrorMessageCodes.ValidationInvalidCharacters)
            .WithState(_ => new { ValidCharacters = UserValidations.UserNameAllowedCharacters })
            .MustAsync(CheckIsUserNameWithSameNameDoesNotExistAsync)
            .WithErrorCode(ErrorMessageCodes.ValidationSameValueExist);
        
        RuleFor(v => v.Email)
            .NotEmpty()
            .WithErrorCode(ErrorMessageCodes.ValidationRequiredValue)
            .Length(UserValidations.EmailMinLength, UserValidations.EmailMaxLength)
            .WithErrorCode(ErrorMessageCodes.ValidationInvalidRange)
            .WithState(_ => new { MinLength = UserValidations.EmailMinLength, MaxLength = UserValidations.EmailMaxLength })
            .EmailAddress()
            .WithErrorCode(ErrorMessageCodes.ValidationInvalidCharacters)
            .MustAsync(CheckIsUserWithSameEmailDoesNotExistAsync)
            .WithErrorCode(ErrorMessageCodes.ValidationSameValueExist);
        
        RuleFor(v => v.Role)
            .NotEmpty()
            .WithErrorCode(ErrorMessageCodes.ValidationRequiredValue)
            .MustAsync(CheckIsRoleAssigningAvailableAsync)
            .WithErrorCode(ErrorMessageCodes.ValidationActionIsNotAllowed);

        RuleFor(v => v.FirstName)
            .Length(UserValidations.FirstNameMinLength, UserValidations.FirstNameMaxLength)
            .WithErrorCode(ErrorMessageCodes.ValidationInvalidRange)
            .WithState(_ => new { MinLength = UserValidations.FirstNameMinLength, MaxLength = UserValidations.LastNameMaxLength })
            .Matches(UserValidations.FirstNameCharactersValidation)
            .WithErrorCode(ErrorMessageCodes.ValidationInvalidCharacters)
            .WithState(_ => new { ValidCharacters = UserValidations.FirstNameAllowedCharacters })
            .When(v => v.FirstName != null);
        
        RuleFor(v => v.LastName)
            .Length(UserValidations.LastNameMinLength, UserValidations.FirstNameMaxLength)
            .WithErrorCode(ErrorMessageCodes.ValidationInvalidRange)
            .WithState(_ => new { MinLength = UserValidations.LastNameMinLength, MaxLength = UserValidations.LastNameMaxLength })
            .Matches(UserValidations.LastNameCharactersValidation)
            .WithErrorCode(ErrorMessageCodes.ValidationInvalidCharacters)
            .WithState(_ => new { ValidCharacters = UserValidations.LastNameAllowedCharacters })
            .When(v => v.LastName != null);
    }

    private async Task<bool> CheckIsUserNameWithSameNameDoesNotExistAsync(CreateUserCommand createUserCommand, string userName, 
        CancellationToken cancellationToken)
    {
        return !await _applicationDbContext.Users.AsNoTracking().AnyAsync(x => x.UserName == userName, cancellationToken);
    }
    
    private async Task<bool> CheckIsUserWithSameEmailDoesNotExistAsync(CreateUserCommand createUserCommand, string email, 
        CancellationToken cancellationToken)
    {
        return !await _applicationDbContext.Users.AsNoTracking().AnyAsync(x => x.Email == email, cancellationToken);
    }
    
    private Task<bool> CheckIsRoleAssigningAvailableAsync(CreateUserCommand createUserCommand, Role? role, CancellationToken cancellationToken)
    {
        return !_sessionService.UserRole.HasValue ? Task.FromResult(false) : Task.FromResult(_sessionService.UserRole < role);
    }
}