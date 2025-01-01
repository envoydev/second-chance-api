using Mapster;
using SecondChance.Api.Presentation.Endpoints.Models;
using SecondChance.Application.CQRS.Projects.Commands.CreateProject;
using SecondChance.Application.CQRS.Projects.Commands.UpdateProject;
using SecondChance.Application.CQRS.Projects.Queries.GetAllProjects;

namespace SecondChance.Api.Presentation.Endpoints.Mappings;

public class ProjectV1Mappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<GetProjectsV1QueryParams, GetAllProjectsQuery>();
        config.NewConfig<CreateProjectV1Body, CreateProjectCommand>();
        config.NewConfig<UpdateProjectV1Body, UpdateProjectCommand>();
    }
}