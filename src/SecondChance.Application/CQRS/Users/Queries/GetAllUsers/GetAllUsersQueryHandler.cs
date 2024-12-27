using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SecondChance.Application.CQRS.Users.Dtos;
using SecondChance.Application.Extensions;
using SecondChance.Application.Persistant;

namespace SecondChance.Application.CQRS.Users.Queries.GetAllUsers;

// ReSharper disable once UnusedType.Global
public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, List<UserResult>>
{
    private readonly IApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;

    public GetAllUsersQueryHandler(IApplicationDbContext applicationDbContext,
        IMapper mapper)
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }

    public async Task<List<UserResult>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await _applicationDbContext.Users.AsNoTracking()
                                               .WhereIf(request.From.HasValue, x => x.CreatedAt >= request.From)
                                               .WhereIf(request.To.HasValue, x => x.CreatedAt <= request.To)
                                               .SkipIf(request.Skip.HasValue, request.Skip.GetValueOrDefault())
                                               .TakeIf(request.Take.HasValue, request.Take.GetValueOrDefault())
                                               .ToListAsync(cancellationToken);

        var usersResponse = _mapper.Map<List<UserResult>>(users);

        return usersResponse;
    }
}