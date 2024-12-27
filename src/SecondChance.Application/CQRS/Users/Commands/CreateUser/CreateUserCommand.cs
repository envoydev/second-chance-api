using MediatR;
using SecondChance.Application.CQRS.Users.Dtos;
using SecondChance.Domain.Enums;

namespace SecondChance.Application.CQRS.Users.Commands.CreateUser;

public sealed record CreateUserCommand(
    string UserName, 
    string Email,
    Role? Role,
    string? FirstName,
    string? LastName)
    : IRequest<UserResult>;