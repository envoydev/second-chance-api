using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using SecondChance.Application.Errors;
using SecondChance.Application.Persistant;
using SecondChance.Application.Services;

namespace SecondChance.Application.CQRS.Authentication.Commands.RefreshToken;

// ReSharper disable once UnusedType.Global
public class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
{
    private readonly IApplicationDbContext _applicationDbContext;
    private readonly IDateTimeService _dateTimeService;
    private readonly ITokenService _tokenService;

    public RefreshTokenCommandValidator(IApplicationDbContext applicationDbContext,
        IDateTimeService dateTimeService,
        ITokenService tokenService)
    {
        _applicationDbContext = applicationDbContext;
        _dateTimeService = dateTimeService;
        _tokenService = tokenService;

        RuleFor(v => v.AccessToken)
            .NotEmpty()
            .WithErrorCode(ErrorMessageCodes.TokenRequired);

        RuleFor(v => v.RefreshToken)
            .NotEmpty()
            .WithErrorCode(ErrorMessageCodes.TokenRequired)
            .MustAsync(CheckRefreshTokenValidWithValidationContextAsync);
    }

    private async Task<bool> CheckRefreshTokenValidWithValidationContextAsync(RefreshTokenCommand command,
        string refreshToken, ValidationContext<RefreshTokenCommand> context, CancellationToken cancellationToken)
    {
        var parsedAccessToken = _tokenService.ParseAccessToken(command.AccessToken);

        if (parsedAccessToken == null)
        {
            context.MessageFormatter.AppendArgument(nameof(ValidationFailure.ErrorCode), ErrorMessageCodes.TokenInvalid);

            return false;
        }

        var currentUser = await _applicationDbContext.Users.AsNoTracking()
                                                     .Where(x => x.Id == parsedAccessToken.UserId)
                                                     .FirstOrDefaultAsync(x => x.RefreshToken == command.RefreshToken, cancellationToken);

        if (currentUser == null)
        {
            context.MessageFormatter.AppendArgument(nameof(ValidationFailure.ErrorCode), ErrorMessageCodes.TokenInvalid);

            return false;
        }

        if (currentUser.RefreshTokenExpiration > _dateTimeService.GetUtc())
        {
            return true;
        }

        context.MessageFormatter.AppendArgument(nameof(ValidationFailure.ErrorCode), ErrorMessageCodes.TokenExpired);

        return false;
    }
}