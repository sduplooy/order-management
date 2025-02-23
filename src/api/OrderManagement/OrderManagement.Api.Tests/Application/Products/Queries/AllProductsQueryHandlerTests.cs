using Microsoft.EntityFrameworkCore;
using OrderManagement.Api.Application.Products.Queries;
using OrderManagement.Api.Domain.Entities;
using OrderManagement.Api.Infrastructure.Database;
using Shouldly;

namespace OrderManagement.Api.UnitTests.Application.Products.Queries;

public class When_calling_handle_on_all_products_query_handler : IDisposable
{
    private readonly AllProductsQueryHandler _queryHandler;
    private readonly OrderManagementDbContext _dbContext;

    public When_calling_handle_on_all_products_query_handler()
    {
        var options = new DbContextOptionsBuilder<OrderManagementDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    
        _dbContext = new OrderManagementDbContext(options);
        _queryHandler = new AllProductsQueryHandler(_dbContext);
    }

    [Fact]
    public async Task It_should_return_all_products()
    {
        var products = new List<Product>
        {
            new() { Id = ProductId.New(), Name = "Product1", Price = 10.0M },
            new() { Id = ProductId.New(), Name = "Product2", Price = 20.0M }
        };
        
        _dbContext.Products.AddRange(products);
        await _dbContext.SaveChangesAsync();

        var result = await _queryHandler.Handle(new AllProductsQuery(), CancellationToken.None);

        result.ShouldNotBeNull();
        result.ShouldContain(p => p.Name == "Product1" && p.Price == 10.0M);
        result.ShouldContain(p => p.Name == "Product2" && p.Price == 20.0M);
    }

    [Fact]
    public async Task It_should_return_empty_list_when_no_products_exist()
    {
        var result = await _queryHandler.Handle(new AllProductsQuery(), CancellationToken.None);

        result.ShouldNotBeNull();
        result.ShouldBeEmpty();
    }

    public void Dispose()
    {
        _dbContext.Dispose();
        GC.SuppressFinalize(this);
    }
}