using Microsoft.EntityFrameworkCore;
using OrderManagement.Api.Application.Products.UpdateProduct;
using OrderManagement.Api.Domain.Entities;
using OrderManagement.Api.Infrastructure.Database;
using Shouldly;

namespace OrderManagement.Api.UnitTests.Application.Products.UpdateProduct;

public class When_calling_handle_on_update_product_handler : IDisposable
{
    private readonly UpdateProductHandler _handler;
    private readonly OrderManagementDbContext _dbContext;

    public When_calling_handle_on_update_product_handler()
    {
        var options = new DbContextOptionsBuilder<OrderManagementDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        
        _dbContext = new OrderManagementDbContext(options);
        _handler = new UpdateProductHandler(_dbContext);
    }

    [Fact]
    public async Task It_should_update_the_product_in_the_database_and_return_a_product_id()
    {
        var productId = ProductId.New();
        
        _dbContext.Products.Add(new Product { Id = productId , Name = "Product", Price = 10.0M });
        await _dbContext.SaveChangesAsync();
        
        var command = new UpdateProductCommand(productId.Value, "Updated Product", 20M);
        var result = await _handler.Handle(command, CancellationToken.None);

        result.ShouldBeOfType<Guid>();
        
        var product = await _dbContext.Products.FindAsync(productId);
        product.Name.ShouldBe("Updated Product");
        product.Price.ShouldBe(20.0M);
    }

    public void Dispose()
    {
        _dbContext.Dispose();
        GC.SuppressFinalize(this);
    }
}