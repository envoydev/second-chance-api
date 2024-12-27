using MediatR;
using SecondChance.Application.CQRS.Projects.Dtos;

namespace SecondChance.Application.CQRS.Projects.Commands.UpdateProject;

public sealed record UpdateProjectCommand(Guid ProjectId, string Name) : IRequest<ProjectResult>;