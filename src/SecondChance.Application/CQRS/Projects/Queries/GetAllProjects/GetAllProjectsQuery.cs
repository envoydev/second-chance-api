using MediatR;
using SecondChance.Application.CQRS.Projects.Dtos;
using SecondChance.Application.Models.Filters;

namespace SecondChance.Application.CQRS.Projects.Queries.GetAllProjects;

public sealed record GetAllProjectsQuery(
    int? Skip, 
    int? Take, 
    DateTime? From, 
    DateTime? To) : IRequest<List<ProjectResult>>, IPagerFilter, IDateRangeFilter;