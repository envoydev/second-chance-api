using SecondChance.Application.Security;
using SecondChance.Domain.Enums;

namespace SecondChance.Api.Presentation.Extensions;

internal static class MinimalApiExtensions
{
    public static TBuilder AuthorizationRequired<TBuilder>(this TBuilder builder, Role[] role) where TBuilder : IEndpointConventionBuilder
    {
        if (builder == null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        builder.Add(convention => convention.Metadata.Add(new AuthorizeAttribute(role)));

        return builder;
    }
}