using MediatR;
using OrderManagement.Api.Domain.Entities;
using OrderManagement.Api.Infrastructure.Database;

namespace OrderManagement.Api.Application.Products.CreateProduct;

public class CreateTodoListCommandHandler(OrderManagementDbContext context)
    : IRequestHandler<CreateProductCommand, ProductId>
{
    public async Task<ProductId> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var product = new Product
        {
            Id = ProductId.New(),
            Name = request.Name,
            Price = request.Price
        };
        
        await context.Products.AddAsync(product, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        
        return product.Id;
    }
}