using MediatR;
using SecondChance.Application.Security;
using SecondChance.Domain.Enums;

namespace SecondChance.Application.CQRS.Transactions.Commands.DeleteTransaction;

[Authorization([Role.Admin, Role.SuperAdmin])]
public sealed record DeleteTransactionCommand(Guid TransactionId) : IRequest;