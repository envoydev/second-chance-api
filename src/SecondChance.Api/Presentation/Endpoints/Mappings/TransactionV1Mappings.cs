using Mapster;
using SecondChance.Api.Presentation.Endpoints.Models;
using SecondChance.Application.CQRS.Transactions.Commands.CreateTransaction;
using SecondChance.Application.CQRS.Transactions.Commands.UpdateTransaction;
using SecondChance.Application.CQRS.Transactions.Queries.GetAllTransactions;

namespace SecondChance.Api.Presentation.Endpoints.Mappings;

public class TransactionV1Mappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<GetTransactionsV1QueryParams, GetAllTransactionsQuery>();
        config.NewConfig<CreateTransactionV1Body, CreateTransactionCommand>();
        config.NewConfig<UpdateTransactionV1Body, UpdateTransactionCommand>();
    }
}