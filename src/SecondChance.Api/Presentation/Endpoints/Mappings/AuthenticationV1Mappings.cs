using Mapster;
using SecondChance.Api.Presentation.Endpoints.Models;
using SecondChance.Application.CQRS.Authentication.Commands.AccessToken;
using SecondChance.Application.CQRS.Authentication.Commands.RefreshToken;

namespace SecondChance.Api.Presentation.Endpoints.Mappings;

public class AuthenticationV1Mappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<LoginV1Body, AccessTokenCommand>();
        config.NewConfig<RefreshV1Body, RefreshTokenCommand>();
    }
}