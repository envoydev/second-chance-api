using MediatR;
using SecondChance.Application.CQRS.Transactions.Dtos;
using SecondChance.Application.Models.Filters;

namespace SecondChance.Application.CQRS.Transactions.Queries.GetAllTransactions;

public sealed record GetAllTransactionsQuery(
    Guid? ProjectId, 
    int? Skip, 
    int? Take, 
    DateTime? From, 
    DateTime? To) : IRequest<List<TransactionResult>>, IPagerFilter, IDateRangeFilter;