using MediatR;

namespace SecondChance.Application.CQRS.Projects.Commands.DeleteProject;

public sealed record DeleteProjectCommand(Guid ProjectId) : IRequest;