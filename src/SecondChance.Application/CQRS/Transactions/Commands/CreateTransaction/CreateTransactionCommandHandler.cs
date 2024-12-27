using MapsterMapper;
using MediatR;
using SecondChance.Application.CQRS.Transactions.Dtos;
using SecondChance.Application.Persistant;
using SecondChance.Domain.Entities;

namespace SecondChance.Application.CQRS.Transactions.Commands.CreateTransaction;

public sealed class CreateTransactionCommandHandler : IRequestHandler<CreateTransactionCommand, TransactionResult>
{
    private readonly IApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;

    public CreateTransactionCommandHandler(IApplicationDbContext applicationDbContext,
        IMapper mapper)
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }

    public async Task<TransactionResult> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
    {
        var transaction = _mapper.Map<Transaction>(request);

        _applicationDbContext.Transactions.Add(transaction);
        await _applicationDbContext.SaveChangesAsync(cancellationToken);

        var transactionResult = _mapper.Map<TransactionResult>(transaction);

        return transactionResult;
    }
}