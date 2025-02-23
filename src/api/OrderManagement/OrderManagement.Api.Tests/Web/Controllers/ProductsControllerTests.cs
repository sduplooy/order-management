using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using OrderManagement.Api.Application.Products.CreateProduct;
using OrderManagement.Api.Application.Products.DeleteProduct;
using OrderManagement.Api.Application.Products.Queries;
using OrderManagement.Api.Application.Products.UpdateProduct;
using OrderManagement.Api.Domain.Entities;
using OrderManagement.Api.Web.Controllers;
using Shouldly;

namespace OrderManagement.Api.UnitTests.Web.Controllers;

public class When_calling_action_methods_on_products_controller
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly ProductsController _controller;

    public When_calling_action_methods_on_products_controller()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new ProductsController(_mediatorMock.Object);
    }

    [Fact]
    public async Task It_should_return_all_products()
    {
        var products = new List<Product>
        {
            new() { Id = new ProductId(Guid.NewGuid()), Name = "Product1", Price = 10.0M },
            new() { Id = new ProductId(Guid.NewGuid()), Name = "Product2", Price = 20.0M }
        };
        _mediatorMock.Setup(m => m.Send(It.IsAny<AllProductsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(products);

        var result = await _controller.GetAllProducts(CancellationToken.None);

        result.ShouldNotBeNull();
        result.Count().ShouldBe(2);
        result.ShouldContain(p => p.Name == "Product1" && p.Price == 10.0M);
        result.ShouldContain(p => p.Name == "Product2" && p.Price == 20.0M);
    }

    [Fact]
    public async Task It_should_create_the_product_and_return_a_created_result()
    {
        var command = new CreateProductCommand("NewProduct", 10.0M);
        var productId = Guid.NewGuid();
        _mediatorMock.Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(productId);

        var result = await _controller.CreateProduct(command, CancellationToken.None);

        result.ShouldNotBeNull();
        result.Value.ShouldBe(productId);
        result.Location.ShouldBe($"/products/{productId}");
    }

    [Fact]
    public async Task It_should_update_the_product_and_return_an_ok()
    {
        var productId = Guid.NewGuid();
        var command = new UpdateProductCommand(productId, "UpdatedProduct", 15.0M);
        _mediatorMock.Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(productId));

        var result = await _controller.UpdateProduct(command, CancellationToken.None);

        result.ShouldNotBeNull();
        result.ShouldBeOfType<Ok>();
    }

    [Fact]
    public async Task It_should_delete_the_product_and_return_an_ok()
    {
        var productId = Guid.NewGuid();
        var command = new DeleteProductCommand(new ProductId(productId));
        _mediatorMock.Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var result = await _controller.DeleteProduct(productId, CancellationToken.None);

        result.ShouldNotBeNull();
        result.ShouldBeOfType<Ok>();
    }
}