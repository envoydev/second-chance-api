using Mapster;
using SecondChance.Api.Presentation.Endpoints.Models;
using SecondChance.Application.CQRS.Users.Queries.GetAllUsers;

namespace SecondChance.Api.Presentation.Endpoints.Mappings;

public class UserV1Mappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<GetUsersV1QueryParams, GetAllUsersQuery>();
    }
}