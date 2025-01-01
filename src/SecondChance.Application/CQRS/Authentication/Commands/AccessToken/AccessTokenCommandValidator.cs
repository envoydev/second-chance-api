using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using SecondChance.Application.Errors;
using SecondChance.Application.Persistant;
using SecondChance.Application.Services;

namespace SecondChance.Application.CQRS.Authentication.Commands.AccessToken;

// ReSharper disable once UnusedType.Global
internal sealed class AccessTokenCommandValidator : AbstractValidator<AccessTokenCommand>
{
    private readonly IApplicationDbContext _applicationDbContext;
    private readonly IPasswordService _passwordService;

    public AccessTokenCommandValidator(IApplicationDbContext applicationDbContext,
        IPasswordService passwordService)
    {
        _applicationDbContext = applicationDbContext;
        _passwordService = passwordService;

        RuleFor(v => v)
            .MustAsync(CheckUserNameAndPasswordAsync)
            .WithErrorCode(ErrorMessageCodes.ValidationWrongUserNameOrPassword)
            .OverridePropertyName(nameof(AccessTokenCommand.UserName));
    }

    private async Task<bool> CheckUserNameAndPasswordAsync(AccessTokenCommand accessTokenCommand, AccessTokenCommand property, 
        ValidationContext<AccessTokenCommand> context, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(accessTokenCommand.UserName) || string.IsNullOrWhiteSpace(accessTokenCommand.Password))
        {
            context.MessageFormatter.AppendArgument(nameof(ValidationFailure.ErrorCode), ErrorMessageCodes.ValidationInvalidValue);
            
            return false;
        }
        
        var currentUser = await _applicationDbContext.Users.AsNoTracking().FirstOrDefaultAsync(x => x.UserName == accessTokenCommand.UserName, cancellationToken);
        if (currentUser == null)
        {
            context.MessageFormatter.AppendArgument(nameof(ValidationFailure.ErrorCode), ErrorMessageCodes.ValidationInvalidValue);
            
            return false;
        }

        var isPasswordVerified = _passwordService.Verify(accessTokenCommand.Password, currentUser.PasswordHash);
        if (!isPasswordVerified)
        {
            context.MessageFormatter.AppendArgument(nameof(ValidationFailure.ErrorCode), ErrorMessageCodes.ValidationInvalidValue);
        }
        
        return isPasswordVerified;
    }
}