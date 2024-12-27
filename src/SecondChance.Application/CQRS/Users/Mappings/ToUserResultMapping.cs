using Mapster;
using SecondChance.Application.CQRS.Users.Dtos;
using SecondChance.Domain.Entities;

namespace SecondChance.Application.CQRS.Users.Mappings;

public class ToUserResultMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<User, Dtos.UserResult>();
    }
}