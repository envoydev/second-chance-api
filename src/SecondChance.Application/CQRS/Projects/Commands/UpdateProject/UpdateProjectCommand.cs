using MediatR;
using SecondChance.Application.CQRS.Projects.Dtos;
using SecondChance.Application.Security;
using SecondChance.Domain.Enums;

namespace SecondChance.Application.CQRS.Projects.Commands.UpdateProject;

[Authorization([Role.Admin, Role.SuperAdmin])]
public sealed record UpdateProjectCommand(Guid ProjectId, string Name) : IRequest<ProjectResult>;