using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SecondChance.Application.CQRS.Users.Validators;
using SecondChance.Application.Errors;
using SecondChance.Application.Persistant;
using SecondChance.Application.Services;
using SecondChance.Application.Validators;

namespace SecondChance.Application.CQRS.Users.Commands.CreateUser;

// ReSharper disable once UnusedType.Global
internal sealed class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    private readonly IApplicationDbContext _applicationDbContext;

    public CreateUserCommandValidator(IApplicationDbContext applicationDbContext, 
        ISessionService sessionService)
    {
        _applicationDbContext = applicationDbContext;

        RuleFor(v => v.UserName)
            .SetValidator(new UserUserNameValidator())
            .MustAsync(CheckIsUserNameWithSameNameDoesNotExistAsync)
            .WithErrorCode(ErrorMessageCodes.ValidationSameValueExist);
        
        RuleFor(v => v.Email)
            .SetValidator(new UserEmailValidator())
            .MustAsync(CheckIsUserWithSameEmailDoesNotExistAsync)
            .WithErrorCode(ErrorMessageCodes.ValidationSameValueExist);
        
        RuleFor(v => v.Role)
            .SetValidator(new UserRoleValidator(sessionService));

        RuleFor(v => v.FirstName)
            .SetValidator(new NullValueValidator<string>(new UserFirstNameValidator()))
            .When(v => v.FirstName != null);
        
        RuleFor(v => v.LastName)
            .SetValidator(new NullValueValidator<string>(new UserLastNameValidator()))
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
}