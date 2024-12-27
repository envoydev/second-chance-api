using MediatR;
using Microsoft.EntityFrameworkCore;
using SecondChance.Application.CQRS.Authentication.Dtos;
using SecondChance.Application.Persistant;
using SecondChance.Application.Services;

namespace SecondChance.Application.CQRS.Authentication.Commands.RefreshToken;

public sealed class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, AccessRefreshTokenResul>
{
    private readonly IApplicationDbContext _applicationDbContext;
    private readonly ITokenService _tokenService;

    public RefreshTokenCommandHandler(IApplicationDbContext applicationDbContext,
        ITokenService tokenService)
    {
        _applicationDbContext = applicationDbContext;
        _tokenService = tokenService;
    }

    public async Task<AccessRefreshTokenResul> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var parsedAccessToken = _tokenService.ParseAccessToken(request.AccessToken);

        var user = await _applicationDbContext.Users.Where(x => x.Id == parsedAccessToken!.UserId)
                                              .FirstAsync(x => x.RefreshToken == request.RefreshToken, cancellationToken);

        var newAccessToken = _tokenService.GenerateAccessToken(user.Id, user.Role);
        var newRefreshToken = _tokenService.GenerateRefreshToken();

        user.RefreshToken = newRefreshToken.Token;
        user.RefreshTokenExpiration = newRefreshToken.ExpiredAt;

        await _applicationDbContext.SaveChangesAsync(cancellationToken);

        return new AccessRefreshTokenResul(newAccessToken, newRefreshToken.Token);
    }
}