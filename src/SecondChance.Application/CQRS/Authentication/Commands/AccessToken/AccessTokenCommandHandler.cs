using MediatR;
using Microsoft.EntityFrameworkCore;
using SecondChance.Application.CQRS.Authentication.Dtos;
using SecondChance.Application.Persistant;
using SecondChance.Application.Services;

namespace SecondChance.Application.CQRS.Authentication.Commands.AccessToken;

public sealed class AccessTokenCommandHandler : IRequestHandler<AccessTokenCommand, AccessRefreshTokenResul>
{
    private readonly IApplicationDbContext _applicationDbContext;
    private readonly ITokenService _tokenService;

    public AccessTokenCommandHandler(IApplicationDbContext applicationDbContext,
        ITokenService tokenService)
    {
        _applicationDbContext = applicationDbContext;
        _tokenService = tokenService;
    }

    public async Task<AccessRefreshTokenResul> Handle(AccessTokenCommand request, CancellationToken cancellationToken)
    {
        var currentUser = await _applicationDbContext.Users.FirstAsync(x => x.UserName == request.UserName, cancellationToken);

        var accessToken = _tokenService.GenerateAccessToken(currentUser.Id, currentUser.Role);
        var refreshToken = _tokenService.GenerateRefreshToken();

        currentUser.RefreshToken = refreshToken.Token;
        currentUser.RefreshTokenExpiration = refreshToken.ExpiredAt;

        await _applicationDbContext.SaveChangesAsync(cancellationToken);

        return new AccessRefreshTokenResul(accessToken, refreshToken.Token);
    }
}