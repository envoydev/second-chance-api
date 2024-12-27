using MediatR;
using SecondChance.Application.CQRS.Transactions.Dtos;
using SecondChance.Domain.Enums;

namespace SecondChance.Application.CQRS.Transactions.Commands.CreateTransaction;

public sealed record CreateTransactionCommand(
    Guid ProjectId,
    OperationType? OperationType,
    CurrencyType? CurrencyType,
    decimal? Amount,
    string? Description) : IRequest<TransactionResult>;