using OrderManagement.Api.Infrastructure.Database;

namespace OrderManagement.Api.Infrastructure.Composition;

internal static class InitializeExtensions
{
    public static WebApplication InitializeDatabase(this WebApplication app)
    {
        CreateDatabaseIfNotExists(app);
        return app;
    }

    private static void CreateDatabaseIfNotExists(WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;

        var context = services.GetRequiredService<OrderManagementDbContext>();
        context.Database.EnsureCreated();
    }
}
