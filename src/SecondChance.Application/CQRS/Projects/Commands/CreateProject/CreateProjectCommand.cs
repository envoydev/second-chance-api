using MediatR;
using SecondChance.Application.CQRS.Projects.Dtos;

namespace SecondChance.Application.CQRS.Projects.Commands.CreateProject;

public sealed record CreateProjectCommand(string Name) : IRequest<ProjectResult>;