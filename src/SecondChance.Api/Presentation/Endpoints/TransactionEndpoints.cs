using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SecondChance.Api.Presentation.Endpoints.Models;
using SecondChance.Api.Presentation.Extensions;
using SecondChance.Application.CQRS.Transactions.Commands.CreateTransaction;
using SecondChance.Application.CQRS.Transactions.Commands.DeleteTransaction;
using SecondChance.Application.CQRS.Transactions.Commands.UpdateTransaction;
using SecondChance.Application.CQRS.Transactions.Queries.GetAllTransactions;
using SecondChance.Domain.Enums;

namespace SecondChance.Api.Presentation.Endpoints;

// ReSharper disable once UnusedType.Global
public class TransactionEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/v{versions:apiVersion}/transactions")
                       .WithApiVersionSet(app.GetApplicationApiVersionSet())
                       .WithOpenApi()
                       .AuthorizationRequired([Role.User]);

        group.MapGet("/", GetTransactionsV1Async)
             .MapToApiVersion(1);
        
        group.MapPost("/", CreateTransactionV1Async)
             .MapToApiVersion(1);
        
        group.MapPut("{id:guid}", UpdateTransactionV1Async)
             .MapToApiVersion(1);
        
        group.MapDelete("{id:guid}", DeleteTransactionsV1Async)
             .MapToApiVersion(1);
    }
    
    private static async Task<IResult> GetTransactionsV1Async([AsParameters] GetTransactionsV1QueryParams queryParams, ISender sender)
    {
        var query = new GetAllTransactionsQuery(queryParams.ProjectId, queryParams.Skip, queryParams.Take, 
            queryParams.From, queryParams.To);
        
        var result = await sender.Send(query);

        return Results.Ok(result);
    }
    
    private static async Task<IResult> CreateTransactionV1Async([FromBody] CreateTransactionV1Body body, ISender sender)
    {
        var command = new CreateTransactionCommand(body.ProjectId, body.OperationType, body.CurrencyType, 
            body.Amount, body.Description);
        
        var result = await sender.Send(command);

        return Results.Ok(result);
    }
    
    private static async Task<IResult> UpdateTransactionV1Async([FromRoute] Guid id, [FromBody] UpdateTransactionV1Body body, ISender sender)
    {
        var command = new UpdateTransactionCommand(id, body.ProjectId, body.OperationType, body.CurrencyType, 
            body.Amount, body.Description);
        
        await sender.Send(command);

        return Results.NoContent();
    } 
    
    private static async Task<IResult> DeleteTransactionsV1Async([FromRoute] Guid id, ISender sender)
    {
        await sender.Send(new DeleteTransactionCommand(id));

        return Results.NoContent();
    }
}