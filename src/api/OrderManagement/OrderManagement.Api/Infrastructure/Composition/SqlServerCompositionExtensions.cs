using Microsoft.EntityFrameworkCore;
using OrderManagement.Api.Infrastructure.Database;

namespace OrderManagement.Api.Infrastructure.Composition;

internal static class SqlServerCompositionExtensions
{
    public static IServiceCollection AddDatabaseContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<OrderManagementDbContext>(options =>
        {
            options.UseSqlServer(GetConnectionString(configuration));
        });

        return services;
    }
    
    public static IHealthChecksBuilder AddDatabaseServerHealthCheck(this IHealthChecksBuilder builder, IConfiguration configuration)
    {
        return builder.AddSqlServer(GetConnectionString(configuration));
    }
    
    private static string GetConnectionString(IConfiguration configuration)
    {
        return configuration.GetConnectionString("Default")!;
    }
}