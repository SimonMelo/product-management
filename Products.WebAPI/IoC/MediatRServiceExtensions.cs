using FluentValidation;
using MediatR;
using Products.WebAPI.Common.Behaviors;

namespace Products.WebAPI.IoC;

public static class MediatRServiceExtensions
{
    public static IServiceCollection AddMediatRService(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(Program).Assembly));
        services.AddValidatorsFromAssembly(typeof(Program).Assembly);
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        
        return services;
    }
}