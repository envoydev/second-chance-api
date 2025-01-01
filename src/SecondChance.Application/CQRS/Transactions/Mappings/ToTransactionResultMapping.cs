using Mapster;
using SecondChance.Application.CQRS.Transactions.Dtos;
using SecondChance.Domain.Entities;

namespace SecondChance.Application.CQRS.Transactions.Mappings;

internal sealed class ToTransactionResultMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Transaction, TransactionResult>();
    }
}