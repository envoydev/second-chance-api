using Mapster;
using SecondChance.Application.CQRS.Transactions.Commands.CreateTransaction;
using SecondChance.Domain.Entities;

namespace SecondChance.Application.CQRS.Transactions.Mappings;

public class ToTransactionMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreateTransactionCommand, Transaction>();
    }
}