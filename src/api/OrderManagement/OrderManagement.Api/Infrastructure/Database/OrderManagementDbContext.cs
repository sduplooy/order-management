using Microsoft.EntityFrameworkCore;

namespace OrderManagement.Api.Infrastructure.Database;

internal class OrderManagementDbContext(DbContextOptions<OrderManagementDbContext> options) : DbContext(options)
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer();
    }
}