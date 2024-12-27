using Mapster;
using SecondChance.Application.CQRS.Users.Commands.CreateUser;
using SecondChance.Domain.Entities;

namespace SecondChance.Application.CQRS.Users.Mappings;

public class ToUserMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreateUserCommand, User>();
    }
}