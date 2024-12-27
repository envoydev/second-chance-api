using MapsterMapper;
using MediatR;
using SecondChance.Application.CQRS.Projects.Dtos;
using SecondChance.Application.Persistant;
using SecondChance.Domain.Entities;

namespace SecondChance.Application.CQRS.Projects.Commands.CreateProject;

public sealed class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, ProjectResult>
{
    private readonly IApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;

    public CreateProjectCommandHandler(IApplicationDbContext applicationDbContext,
        IMapper mapper)
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }

    public async Task<ProjectResult> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
    {
        var newProject = _mapper.Map<Project>(request);

        _applicationDbContext.Projects.Add(newProject);
        await _applicationDbContext.SaveChangesAsync(cancellationToken);

        var projectResult = _mapper.Map<ProjectResult>(newProject);

        return projectResult;
    }
}