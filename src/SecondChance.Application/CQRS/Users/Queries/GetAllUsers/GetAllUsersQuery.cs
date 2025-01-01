using MediatR;
using SecondChance.Application.CQRS.Users.Dtos;
using SecondChance.Application.Models.QueryFilters;
using SecondChance.Application.Security;
using SecondChance.Domain.Enums;

namespace SecondChance.Application.CQRS.Users.Queries.GetAllUsers;

[Authorization([Role.Admin, Role.SuperAdmin])]
public sealed record GetAllUsersQuery(
    int? Skip, 
    int? Take, 
    DateTime? From, 
    DateTime? To) : IRequest<List<UserResult>>, IPagerQueryFilter, IDateRangeQueryFilter;