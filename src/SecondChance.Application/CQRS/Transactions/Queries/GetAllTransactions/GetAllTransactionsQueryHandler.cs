using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SecondChance.Application.CQRS.Transactions.Dtos;
using SecondChance.Application.Extensions;
using SecondChance.Application.Persistant;

namespace SecondChance.Application.CQRS.Transactions.Queries.GetAllTransactions;

public sealed class GetAllTransactionsQueryHandler : IRequestHandler<GetAllTransactionsQuery, List<TransactionResult>>
{
    private readonly IApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;

    public GetAllTransactionsQueryHandler(IApplicationDbContext applicationDbContext,
        IMapper mapper)
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }

    public async Task<List<TransactionResult>> Handle(GetAllTransactionsQuery request, CancellationToken cancellationToken)
    {
        var transactions = await _applicationDbContext.Transactions.AsNoTracking()
                                                      .WhereIf(request.ProjectId.HasValue, x => x.ProjectId == request.ProjectId!.Value)
                                                      .WhereIf(request.From.HasValue, x => x.CreatedAt >= request.From)
                                                      .WhereIf(request.To.HasValue, x => x.CreatedAt <= request.To)
                                                      .SkipIf(request.Skip.HasValue, request.Skip.GetValueOrDefault())
                                                      .TakeIf(request.Take.HasValue, request.Take.GetValueOrDefault())
                                                      .ToListAsync(cancellationToken);

        var transactionDtos = _mapper.Map<List<TransactionResult>>(transactions);

        return transactionDtos;
    }
}