using Microsoft.EntityFrameworkCore;
using OrderManagement.Api.Domain.Entities;
using OrderManagement.Api.Domain.ValueObjects;
using OrderManagement.Api.Infrastructure.Database.ValueConverters;

namespace OrderManagement.Api.Infrastructure.Database;

public class OrderManagementDbContext(DbContextOptions<OrderManagementDbContext> options) : DbContext(options)
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.UseValueConverter(new ProductIdValueConverter());
        modelBuilder.UseValueConverter(new OrderIdValueConverter());
        
        modelBuilder.Entity<OrderLineItem>()
            .HasKey(l => new { l.OrderId, l.ProductId })
            .HasName("Id");
    }

    public DbSet<Product> Products => Set<Product>();
    public DbSet<Order> Orders => Set<Order>();
}
