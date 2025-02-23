using Microsoft.EntityFrameworkCore;
using Moq;
using OrderManagement.Api.Application.Products.CreateProduct;
using OrderManagement.Api.Domain.Entities;
using OrderManagement.Api.Infrastructure.Database;
using Shouldly;

namespace OrderManagement.Api.UnitTests.Application.Products.CreateProduct;

public class When_calling_handle_on_create_product_handler : IDisposable
{
    private readonly CreateProductHandler _handler;
    private readonly OrderManagementDbContext _dbContext;

    public When_calling_handle_on_create_product_handler()
    {
        var options = new DbContextOptionsBuilder<OrderManagementDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        
        _dbContext = new OrderManagementDbContext(options);
        _handler = new CreateProductHandler(_dbContext);
    }

    [Fact]
    public async Task It_should_add_the_product_to_the_database_and_return_a_product_id()
    {
        var command = new CreateProductCommand("NewProduct", 10M);
        var productId = await _handler.Handle(command, CancellationToken.None);
        
        var product = await _dbContext.Products.FindAsync(new ProductId(productId));
        product.Name.ShouldBe("NewProduct");
        product.Price.ShouldBe(10.0M);
    }

    [Fact]
    public async Task It_should_return_a_product_id()
    {
        var command = new CreateProductCommand("NewProduct", 10M);
        var productId = await _handler.Handle(command, CancellationToken.None);

        productId.ShouldBeOfType<Guid>();
    }

    public void Dispose()
    {
        _dbContext.Dispose();
        GC.SuppressFinalize(this);
    }
}