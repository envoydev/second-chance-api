using MediatR;
using SecondChance.Application.CQRS.Authentication.Dtos;

namespace SecondChance.Application.CQRS.Authentication.Commands.RefreshToken;

public sealed record RefreshTokenCommand(string AccessToken, string RefreshToken) : IRequest<AccessRefreshTokenResul>;