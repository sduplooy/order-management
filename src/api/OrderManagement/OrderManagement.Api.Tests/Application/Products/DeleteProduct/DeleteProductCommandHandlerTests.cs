using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Moq;
using OrderManagement.Api.Application.Products.DeleteProduct;
using OrderManagement.Api.Domain.Entities;
using OrderManagement.Api.Infrastructure.Database;
using Shouldly;

namespace OrderManagement.Api.UnitTests.Application.Products.DeleteProduct;

public class When_calling_handler_on_delete_product_command_handler
{
    private readonly DeleteProductCommandHandler _handler;
    private readonly OrderManagementDbContext _dbContext;

    public When_calling_handler_on_delete_product_command_handler()
    {
        var options = new DbContextOptionsBuilder<OrderManagementDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        
        _dbContext = new OrderManagementDbContext(options);
        _handler = new DeleteProductCommandHandler(_dbContext);
    }

    [Fact]
    public async Task It_should_remove_the_product_from_the_database()
    {
        var productId = new ProductId(Guid.NewGuid());
        await _dbContext.AddAsync(new Product { Id = productId, Name = "Product", Price = 10.0M });
        
        await _handler.Handle(new DeleteProductCommand(productId), CancellationToken.None);

        var product = await _dbContext.Products.SingleOrDefaultAsync(p => p.Id == productId);
        product.ShouldBeNull();
    }

    [Fact]
    public async Task It_should_not_throw_when_product_not_found()
    {
        var productId = new ProductId(Guid.NewGuid());
        
        await _handler.Handle(new DeleteProductCommand(productId), CancellationToken.None).ShouldNotThrowAsync();
    }
}