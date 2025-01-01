using Mapster;
using SecondChance.Application.CQRS.Projects.Commands.CreateProject;
using SecondChance.Application.CQRS.Projects.Commands.UpdateProject;
using SecondChance.Domain.Entities;

namespace SecondChance.Application.CQRS.Projects.Mappings;

internal sealed class ToProjectMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<UpdateProjectCommand, Project>();
        config.NewConfig<CreateProjectCommand, Project>();
    }
}