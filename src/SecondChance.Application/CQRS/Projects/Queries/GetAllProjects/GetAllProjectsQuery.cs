using MediatR;
using SecondChance.Application.CQRS.Projects.Dtos;
using SecondChance.Application.Models.QueryFilters;
using SecondChance.Application.Security;
using SecondChance.Domain.Enums;

namespace SecondChance.Application.CQRS.Projects.Queries.GetAllProjects;

[Authorization([Role.User, Role.Admin, Role.SuperAdmin])]
public sealed record GetAllProjectsQuery(
    int? Skip, 
    int? Take, 
    DateTime? From, 
    DateTime? To) : IRequest<List<ProjectResult>>, IPagerQueryFilter, IDateRangeQueryFilter;