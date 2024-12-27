using MediatR;
using Microsoft.EntityFrameworkCore;
using SecondChance.Application.Persistant;

namespace SecondChance.Application.CQRS.Transactions.Commands.DeleteTransaction;

public sealed class DeleteTransactionCommandHandler : IRequestHandler<DeleteTransactionCommand>
{
    private readonly IApplicationDbContext _applicationDbContext;

    public DeleteTransactionCommandHandler(IApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public async Task Handle(DeleteTransactionCommand request, CancellationToken cancellationToken)
    {
        var transaction = await _applicationDbContext.Transactions.FirstAsync(x => x.Id == request.TransactionId, cancellationToken);

        _applicationDbContext.Transactions.Remove(transaction);
        
        await _applicationDbContext.SaveChangesAsync(cancellationToken);
    }
}