using SecondChance.Application.Security;

namespace SecondChance.Api.Presentation.Extensions;

internal static class MinimalApiExtensions
{
    public static TBuilder AuthorizationRequired<TBuilder>(this TBuilder builder) where TBuilder : IEndpointConventionBuilder
    {
        if (builder == null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        builder.Add(convention => convention.Metadata.Add(new AuthorizationAttribute()));

        return builder;
    }
}