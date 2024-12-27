using MediatR;
using SecondChance.Application.CQRS.Transactions.Dtos;
using SecondChance.Domain.Enums;

namespace SecondChance.Application.CQRS.Transactions.Commands.UpdateTransaction;

public sealed record UpdateTransactionCommand(
    Guid TransactionId,
    Guid ProjectId,
    OperationType? OperationType,
    CurrencyType? CurrencyType,
    decimal? Amount,
    string? Description) : IRequest<TransactionResult>;