using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SecondChance.Application.CQRS.Projects.Dtos;
using SecondChance.Application.Persistant;

namespace SecondChance.Application.CQRS.Projects.Commands.UpdateProject;

public sealed class UpdateProjectCommandHandler : IRequestHandler<UpdateProjectCommand, ProjectResult>
{
    private readonly IApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;

    public UpdateProjectCommandHandler(IApplicationDbContext applicationDbContext,
        IMapper mapper)
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }

    public async Task<ProjectResult> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
    {
        var project = await _applicationDbContext.Projects.FirstAsync(x => x.Id == request.ProjectId, cancellationToken);

        project.Name = request.Name;

        await _applicationDbContext.SaveChangesAsync(cancellationToken);

        var projectResult = _mapper.Map<ProjectResult>(project);

        return projectResult;
    }
}