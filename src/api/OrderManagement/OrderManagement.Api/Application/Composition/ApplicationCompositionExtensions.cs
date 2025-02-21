using System.Reflection;
using FluentValidation;
using MediatR;
using OrderManagement.Api.Application.Behaviours;

namespace OrderManagement.Api.Application.Composition;

internal static class ApplicationCompositionExtensions
{
    public static IServiceCollection AddOrderManagementApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(IApiAssemblyMarker).Assembly);
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        });
        
        return services;
    }
}