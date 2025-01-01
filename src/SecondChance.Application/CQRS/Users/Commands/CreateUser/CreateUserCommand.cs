using MediatR;
using SecondChance.Application.CQRS.Users.Dtos;
using SecondChance.Application.Security;
using SecondChance.Domain.Enums;
using AppRole = SecondChance.Domain.Enums.Role;

namespace SecondChance.Application.CQRS.Users.Commands.CreateUser;

[Authorization([AppRole.Admin, AppRole.SuperAdmin])]
public sealed record CreateUserCommand(
    string UserName, 
    string Email,
    AppRole? Role,
    string? FirstName,
    string? LastName) : IRequest<UserResult>;