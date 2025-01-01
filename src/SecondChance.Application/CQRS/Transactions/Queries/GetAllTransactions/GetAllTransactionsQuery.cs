using MediatR;
using SecondChance.Application.CQRS.Transactions.Dtos;
using SecondChance.Application.Models.QueryFilters;
using SecondChance.Application.Security;
using SecondChance.Domain.Enums;

namespace SecondChance.Application.CQRS.Transactions.Queries.GetAllTransactions;

[Authorization([Role.User, Role.Admin, Role.SuperAdmin])]
public sealed record GetAllTransactionsQuery(
    Guid? ProjectId, 
    int? Skip, 
    int? Take, 
    DateTime? From, 
    DateTime? To) : IRequest<List<TransactionResult>>, IPagerQueryFilter, IDateRangeQueryFilter;