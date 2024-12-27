using MediatR;
using SecondChance.Application.CQRS.Authentication.Dtos;

namespace SecondChance.Application.CQRS.Authentication.Commands.AccessToken;

public record AccessTokenCommand(string UserName, string Password) : IRequest<AccessRefreshTokenResul>;