using MediatR;

namespace SecondChance.Application.CQRS.Transactions.Commands.DeleteTransaction;

public sealed record DeleteTransactionCommand(Guid TransactionId) : IRequest;