using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SecondChance.Application.CQRS.Projects.Dtos;
using SecondChance.Application.Extensions;
using SecondChance.Application.Persistant;

namespace SecondChance.Application.CQRS.Projects.Queries.GetAllProjects;

public sealed class GetAllProjectsQueryHandler : IRequestHandler<GetAllProjectsQuery, List<ProjectResult>>
{
    private readonly IApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;

    public GetAllProjectsQueryHandler(IApplicationDbContext applicationDbContext, IMapper mapper)
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }

    public async Task<List<ProjectResult>> Handle(GetAllProjectsQuery request, CancellationToken cancellationToken)
    {
        var projects = await _applicationDbContext.Projects.AsNoTracking()
                                                  .WhereIf(request.From.HasValue, x => x.CreatedAt >= request.From)
                                                  .WhereIf(request.To.HasValue, x => x.CreatedAt <= request.To)
                                                  .SkipIf(request.Skip.HasValue, request.Skip.GetValueOrDefault())
                                                  .TakeIf(request.Take.HasValue, request.Take.GetValueOrDefault())
                                                  .ToListAsync(cancellationToken);

        var projectDtos = _mapper.Map<List<ProjectResult>>(projects);

        return projectDtos;
    }
}