using Carter;
using MediatR;
using SecondChance.Api.Presentation.Endpoints.Models;
using SecondChance.Api.Presentation.Extensions;
using SecondChance.Application.CQRS.Users.Queries.GetAllUsers;
using SecondChance.Domain.Enums;

namespace SecondChance.Api.Presentation.Endpoints;

public class UserEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/v{versions:apiVersion}/users")
                       .WithApiVersionSet(app.GetApplicationApiVersionSet())
                       .WithOpenApi()
                       .AuthorizationRequired([Role.Admin, Role.User]);

        group.MapGet("/", GetAllUsers)
             .MapToApiVersion(1);
    }

    private static async Task<IResult> GetAllUsers([AsParameters] GetUsersV1QueryParams queryParams, ISender sender)
    {
        var query = new GetAllUsersQuery(queryParams.Skip, queryParams.Take, queryParams.From, queryParams.To);
        
        var result = await sender.Send(query);

        return Results.Ok(result);
    }
}