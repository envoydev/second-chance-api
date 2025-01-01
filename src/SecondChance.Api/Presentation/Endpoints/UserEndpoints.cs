using Carter;
using MapsterMapper;
using MediatR;
using SecondChance.Api.Presentation.Endpoints.Models;
using SecondChance.Api.Presentation.Extensions;
using SecondChance.Application.CQRS.Users.Queries.GetAllUsers;

namespace SecondChance.Api.Presentation.Endpoints;

public class UserEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/v{versions:apiVersion}/users")
                       .WithApiVersionSet(app.GetApplicationApiVersionSet())
                       .WithOpenApi()
                       .AuthorizationRequired();

        group.MapGet("/", GetAllUsers)
             .MapToApiVersion(1);
    }

    private static async Task<IResult> GetAllUsers([AsParameters] GetUsersV1QueryParams queryParams, ISender sender, IMapper mapper)
    {
        var getAllUsersQuery = mapper.Map<GetAllUsersQuery>(queryParams);
        
        var result = await sender.Send(getAllUsersQuery);

        return Results.Ok(result);
    }
}