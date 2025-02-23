using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.Api.Application.Products.CreateProduct;
using OrderManagement.Api.Application.Products.DeleteProduct;
using OrderManagement.Api.Application.Products.Queries;
using OrderManagement.Api.Application.Products.UpdateProduct;
using OrderManagement.Api.Domain.Entities;

namespace OrderManagement.Api.Web.Controllers;

[Route("products")]
public class ProductsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public Task<IEnumerable<Product>> GetAllProducts(CancellationToken cancellationToken)
    {
        return mediator.Send(new AllProductsQuery(), cancellationToken);
    }
    
    [HttpPost]
    [Consumes("application/json")]
    public async Task<Created<Guid>> CreateProduct([FromBody] CreateProductCommand command, CancellationToken cancellationToken)
    {
        var id = await mediator.Send(command, cancellationToken);
        return TypedResults.Created($"/products/{id}", id);
    }
    
    [HttpPut]
    [Consumes("application/json")]
    public async Task<Ok> UpdateProduct([FromBody] UpdateProductCommand command, CancellationToken cancellationToken)
    {
        await mediator.Send(command, cancellationToken);
        return TypedResults.Ok();
    }
    
    [HttpDelete("{productId}")]
    public async Task<Ok> DeleteProduct([FromRoute] Guid productId, CancellationToken cancellationToken)
    {
        var command = new DeleteProductCommand(new ProductId(productId));
        await mediator.Send(command, cancellationToken);
        return TypedResults.Ok();
    }
}