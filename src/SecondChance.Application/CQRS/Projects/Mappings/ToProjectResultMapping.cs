using Mapster;
using SecondChance.Application.CQRS.Projects.Dtos;
using SecondChance.Domain.Entities;

namespace SecondChance.Application.CQRS.Projects.Mappings;

public class ToProjectResultMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Project, ProjectResult>();
    }
}