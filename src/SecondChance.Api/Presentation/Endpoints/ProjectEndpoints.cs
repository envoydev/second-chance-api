using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SecondChance.Api.Presentation.Endpoints.Models;
using SecondChance.Api.Presentation.Extensions;
using SecondChance.Application.CQRS.Projects.Commands.CreateProject;
using SecondChance.Application.CQRS.Projects.Commands.DeleteProject;
using SecondChance.Application.CQRS.Projects.Commands.UpdateProject;
using SecondChance.Application.CQRS.Projects.Queries.GetAllProjects;
using SecondChance.Domain.Enums;

namespace SecondChance.Api.Presentation.Endpoints;

// ReSharper disable once UnusedType.Global
public class ProjectEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/v{versions:apiVersion}/projects")
                       .WithApiVersionSet(app.GetApplicationApiVersionSet())
                       .WithOpenApi()
                       .AuthorizationRequired([Role.User]);

        group.MapGet("/", GetAllProjectsV1Async)
             .MapToApiVersion(1);
        
        group.MapPost("/", CreateProjectV1Async)
             .MapToApiVersion(1);
        
        group.MapPut("{id:guid}", UpdateProjectV1Async)
             .MapToApiVersion(1);
        
        group.MapDelete("{id:guid}", DeleteProjectV1Async)
             .MapToApiVersion(1);
    }
    
    private static async Task<IResult> GetAllProjectsV1Async([AsParameters] GetProjectsV1QueryParams queryParams, ISender sender)
    {
         var query = new GetAllProjectsQuery(queryParams.Skip, queryParams.Take, queryParams.From, queryParams.To);
         
         var result = await sender.Send(query);

         return Results.Ok(result);
    }
    
    private static async Task<IResult> CreateProjectV1Async([FromBody] CreateProjectV1Body body, ISender sender)
    {
         var result = await sender.Send(new CreateProjectCommand(body.Name));

         return Results.Ok(result);
    }
    
    private static async Task<IResult> UpdateProjectV1Async([FromRoute] Guid id, [FromBody] UpdateProjectV1Body body, ISender sender)
    {
         await sender.Send(new UpdateProjectCommand(id, body.Name));

         return Results.NoContent();
    } 
    
    private static async Task<IResult> DeleteProjectV1Async([FromRoute] Guid id, ISender sender)
    {
         await sender.Send(new DeleteProjectCommand(id));

         return Results.NoContent();
    }
}