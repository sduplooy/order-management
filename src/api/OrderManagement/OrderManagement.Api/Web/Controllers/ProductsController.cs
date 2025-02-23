using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.Api.Application.Products.CreateProduct;
using OrderManagement.Api.Application.Products.Queries;
using OrderManagement.Api.Domain.Entities;

namespace OrderManagement.Api.Web.Controllers;

[Route("products")]
public class ProductsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public Task<IEnumerable<Product>> GetProducts(CancellationToken cancellationToken)
    {
        return mediator.Send(new FetchAllProductsQuery(), cancellationToken);
    }
    
    [HttpPost]
    [Consumes("application/json")]
    public async Task<Created<Guid>> CreateProduct([FromBody] CreateProductCommand command, CancellationToken cancellationToken)
    {
        var id = await mediator.Send(command, cancellationToken);
        return TypedResults.Created($"/products/{id}", id);
    }
}