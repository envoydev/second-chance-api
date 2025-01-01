using Carter;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SecondChance.Api.Presentation.Endpoints.Models;
using SecondChance.Api.Presentation.Extensions;
using SecondChance.Application.CQRS.Transactions.Commands.CreateTransaction;
using SecondChance.Application.CQRS.Transactions.Commands.DeleteTransaction;
using SecondChance.Application.CQRS.Transactions.Commands.UpdateTransaction;
using SecondChance.Application.CQRS.Transactions.Queries.GetAllTransactions;

namespace SecondChance.Api.Presentation.Endpoints;

// ReSharper disable once UnusedType.Global
public class TransactionEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/v{versions:apiVersion}/transactions")
                       .WithApiVersionSet(app.GetApplicationApiVersionSet())
                       .WithOpenApi()
                       .AuthorizationRequired();

        group.MapGet("/", GetTransactionsV1Async)
             .MapToApiVersion(1);
        
        group.MapPost("/", CreateTransactionV1Async)
             .MapToApiVersion(1);
        
        group.MapPut("{id:guid}", UpdateTransactionV1Async)
             .MapToApiVersion(1);
        
        group.MapDelete("{id:guid}", DeleteTransactionsV1Async)
             .MapToApiVersion(1);
    }
    
    private static async Task<IResult> GetTransactionsV1Async([AsParameters] GetTransactionsV1QueryParams queryParams, ISender sender, IMapper mapper)
    {
        var getAllTransactionsQuery = mapper.Map<GetAllTransactionsQuery>(queryParams);
        
        var result = await sender.Send(getAllTransactionsQuery);

        return Results.Ok(result);
    }
    
    private static async Task<IResult> CreateTransactionV1Async([FromBody] CreateTransactionV1Body body, ISender sender, IMapper mapper)
    {
        var createTransactionCommand = mapper.Map<CreateTransactionCommand>(body);
        
        var result = await sender.Send(createTransactionCommand);

        return Results.Ok(result);
    }
    
    private static async Task<IResult> UpdateTransactionV1Async([FromRoute] Guid id, [FromBody] UpdateTransactionV1Body body, ISender sender, IMapper mapper)
    {
        var updateTransactionCommand = mapper.Map<UpdateTransactionCommand>(body) with { TransactionId = id };
        
        await sender.Send(updateTransactionCommand);

        return Results.NoContent();
    } 
    
    private static async Task<IResult> DeleteTransactionsV1Async([FromRoute] Guid id, ISender sender)
    {
        await sender.Send(new DeleteTransactionCommand(id));

        return Results.NoContent();
    }
}