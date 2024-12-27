using FluentValidation;
using Mapster;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using SecondChance.Application.Behaviors;

namespace SecondChance.Application;

public static class DependencyInjection
{
    public static void AddApplication(this IServiceCollection services)
    {
        var assembly = typeof(DependencyInjection).Assembly;
        
        // FluentValidation
        services.AddValidatorsFromAssembly(assembly);
        ValidatorOptions.Global.DefaultClassLevelCascadeMode = CascadeMode.Stop;
        
        // MediatR
        services.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssemblies(assembly);
            configuration.AddBehavior(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
            configuration.AddBehavior(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehaviour<,>));
            configuration.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            configuration.AddBehavior(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));
        });

        // Mapper
        TypeAdapterConfig.GlobalSettings.Scan(assembly);
        services.AddMapster();
    }
}