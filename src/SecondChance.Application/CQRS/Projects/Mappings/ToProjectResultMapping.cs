using Mapster;
using SecondChance.Application.CQRS.Projects.Dtos;
using SecondChance.Domain.Entities;

namespace SecondChance.Application.CQRS.Projects.Mappings;

internal sealed class ToProjectResultMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Project, ProjectResult>();
    }
}