using FluentValidation;
using SecondChance.Application.Errors;
using SecondChance.Application.Services;
using SecondChance.Domain.Enums;

namespace SecondChance.Application.CQRS.Users.Validators;

internal class UserRoleValidator : AbstractValidator<Role?>
{
    private readonly ISessionService _sessionService;
    
    public UserRoleValidator(ISessionService sessionService)
    {
        _sessionService = sessionService;
        
        RuleFor(role => role)
            .NotEmpty()
            .WithErrorCode(ErrorMessageCodes.ValidationRequiredValue)
            .MustAsync(CheckIsRoleAssigningAvailableAsync)
            .WithErrorCode(ErrorMessageCodes.ValidationActionIsNotAllowed);
    }
    
    private Task<bool> CheckIsRoleAssigningAvailableAsync(Role? roleObject, Role? role, CancellationToken cancellationToken)
    {
        return !_sessionService.UserRole.HasValue ? Task.FromResult(false) : Task.FromResult(_sessionService.UserRole < role);
    }
}