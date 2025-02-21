namespace OrderManagement.Api.Application.Composition;

internal static class ApplicationCompositionExtensions
{
    public static IServiceCollection AddOrderManagementApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(IApiAssemblyMarker).Assembly));
        return services;
    }
}