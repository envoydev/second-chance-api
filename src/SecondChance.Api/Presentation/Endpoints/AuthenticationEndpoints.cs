using Carter;
using MediatR;
using SecondChance.Api.Presentation.Endpoints.Models;
using SecondChance.Api.Presentation.Extensions;
using SecondChance.Application.CQRS.Authentication.Commands.AccessToken;
using SecondChance.Application.CQRS.Authentication.Commands.RefreshToken;

namespace SecondChance.Api.Presentation.Endpoints;

// ReSharper disable once UnusedType.Global
public class AuthenticationEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/v{versions:apiVersion}/authentication")
                       .WithApiVersionSet(app.GetApplicationApiVersionSet())
                       .WithOpenApi();

        group.MapPost("login", LoginV1Async)
             .MapToApiVersion(1);
        
        group.MapPost("refresh", RefreshV1Async)
             .MapToApiVersion(1);
    }

    private static async Task<IResult> LoginV1Async(LoginV1Body body, ISender sender)
    {
        var result = await sender.Send(new AccessTokenCommand(body.UserName, body.Password));

        return Results.Ok(result);
    }
    
    private static async Task<IResult> RefreshV1Async(RefreshV1Body body, ISender sender)
    {
        var result = await sender.Send(new RefreshTokenCommand(body.AccessToken, body.RefreshToken));

        return Results.Ok(result);
    }
}