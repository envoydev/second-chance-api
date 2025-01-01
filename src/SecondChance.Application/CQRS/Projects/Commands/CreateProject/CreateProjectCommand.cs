using MediatR;
using SecondChance.Application.CQRS.Projects.Dtos;
using SecondChance.Application.Security;
using SecondChance.Domain.Enums;

namespace SecondChance.Application.CQRS.Projects.Commands.CreateProject;

[Authorization([Role.Admin, Role.SuperAdmin])]
public sealed record CreateProjectCommand(string Name) : IRequest<ProjectResult>;