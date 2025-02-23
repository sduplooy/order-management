using FluentValidation.TestHelper;
using Microsoft.EntityFrameworkCore;
using OrderManagement.Api.Application.Products.CreateProduct;
using OrderManagement.Api.Application.Products.UpdateProduct;
using OrderManagement.Api.Domain.Entities;
using OrderManagement.Api.Infrastructure.Database;

namespace OrderManagement.Api.UnitTests.Application.Products.UpdateProduct;

public class When_calling_validate_on_update_product_command_validator : IDisposable
{
    private readonly UpdateProductCommandValidator _validator;
    private readonly OrderManagementDbContext _dbContext;

    public When_calling_validate_on_update_product_command_validator()
    {
        var options = new DbContextOptionsBuilder<OrderManagementDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        
        _dbContext = new OrderManagementDbContext(options);
        _validator = new UpdateProductCommandValidator(_dbContext);
    }
    
    [Fact]
    public async Task It_should_have_an_error_when_product_name_is_empty()
    {
        var command = new UpdateProductCommand(Guid.Empty, string.Empty, 10.0M);
        var result = await _validator.TestValidateAsync(command);
        result.ShouldHaveValidationErrorFor(c => c.Name);
    }

    [Fact]
    public async Task It_should_have_an_error_when_product_name_exceeds_max_length()
    {
        var command = new UpdateProductCommand(Guid.Empty, new string('a', 201), 10.0M);
        var result = await _validator.TestValidateAsync(command);
        result.ShouldHaveValidationErrorFor(c => c.Name);
    }

    [Fact]
    public async Task It_should_have_an_error_when_product_name_is_not_unique()
    {
        var productId = ProductId.New();
        _dbContext.Products.Add(new Product { Id = productId, Name = "ExistingProduct", Price = 10.0M });
        await _dbContext.SaveChangesAsync();
        
        var command = new UpdateProductCommand(productId.Value, "ExistingProduct", 10.0M);
        
        var result = await _validator.TestValidateAsync(command);
        result.ShouldHaveValidationErrorFor(c => c.Name);
    }

    [Fact]
    public async Task It_should_have_an_error_when_price_is_negative()
    {
        var command = new UpdateProductCommand(Guid.Empty, "Product", -10.00M);
        var result = await _validator.TestValidateAsync(command);
        result.ShouldHaveValidationErrorFor(c => c.Price);
    }

    [Fact]
    public async Task Should_not_have_an_error_when_command_is_valid()
    {
        var command = new UpdateProductCommand(Guid.Empty, "NewProduct", 10.0M);

        var result = await _validator.TestValidateAsync(command);
        result.ShouldNotHaveValidationErrorFor(c => c.Name);
        result.ShouldNotHaveValidationErrorFor(c => c.Price);
    }

    public void Dispose()
    {
        _dbContext.Dispose();
        GC.SuppressFinalize(this);
    }
}