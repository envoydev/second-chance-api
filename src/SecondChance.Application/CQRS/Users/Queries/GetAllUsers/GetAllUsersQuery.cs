using MediatR;
using SecondChance.Application.CQRS.Users.Dtos;
using SecondChance.Application.Models.Filters;
using SecondChance.Application.Security;
using SecondChance.Domain.Enums;

namespace SecondChance.Application.CQRS.Users.Queries.GetAllUsers;

[Authorize([Role.Admin])]
public sealed record GetAllUsersQuery(
    int? Skip, 
    int? Take, 
    DateTime? From, 
    DateTime? To) : IRequest<List<UserResult>>, IPagerFilter, IDateRangeFilter;