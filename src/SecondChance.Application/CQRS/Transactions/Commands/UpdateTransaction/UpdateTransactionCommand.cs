using MediatR;
using SecondChance.Application.CQRS.Transactions.Common;
using SecondChance.Application.CQRS.Transactions.Dtos;
using SecondChance.Application.Security;
using SecondChance.Domain.Enums;

namespace SecondChance.Application.CQRS.Transactions.Commands.UpdateTransaction;

[Authorization([Role.Admin, Role.SuperAdmin])]
public sealed record UpdateTransactionCommand(
    Guid TransactionId,
    Guid ProjectId,
    OperationType? OperationType,
    int? CurrencyCode,
    string? CryptoCurrencyCode,
    decimal Amount,
    string? Description) : ITransactionCurrency, IRequest<TransactionResult>;