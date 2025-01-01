using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SecondChance.Application.CQRS.Transactions.Dtos;
using SecondChance.Application.Persistant;

namespace SecondChance.Application.CQRS.Transactions.Commands.UpdateTransaction;

public sealed class UpdateTransactionCommandHandler : IRequestHandler<UpdateTransactionCommand, TransactionResult>
{
    private readonly IApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;

    public UpdateTransactionCommandHandler(IApplicationDbContext applicationDbContext, 
        IMapper mapper)
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }

    public async Task<TransactionResult> Handle(UpdateTransactionCommand request, CancellationToken cancellationToken)
    {
        var transactionToUpdate = await _applicationDbContext.Transactions.FirstAsync(x => x.Id == request.TransactionId, cancellationToken);

        transactionToUpdate = _mapper.Map(request, transactionToUpdate);

        await _applicationDbContext.SaveChangesAsync(cancellationToken);

        var projectResult = _mapper.Map<TransactionResult>(transactionToUpdate);

        return projectResult;
    }
}