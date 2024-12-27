using MediatR;
using Microsoft.EntityFrameworkCore;
using SecondChance.Application.Persistant;

namespace SecondChance.Application.CQRS.Projects.Commands.DeleteProject;

public sealed class DeleteProjectCommandHandler : IRequestHandler<DeleteProjectCommand>
{
    private readonly IApplicationDbContext _applicationDbContext;

    public DeleteProjectCommandHandler(IApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public async Task Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
    {
        var project = await _applicationDbContext.Projects.FirstAsync(x => x.Id == request.ProjectId, cancellationToken);

        _applicationDbContext.Projects.Remove(project);
        
        await _applicationDbContext.SaveChangesAsync(cancellationToken);
    }
}