using MediatR;
using SecondChance.Application.Security;
using SecondChance.Domain.Enums;

namespace SecondChance.Application.CQRS.Projects.Commands.DeleteProject;

[Authorization([Role.Admin, Role.SuperAdmin])]
public sealed record DeleteProjectCommand(Guid ProjectId) : IRequest;