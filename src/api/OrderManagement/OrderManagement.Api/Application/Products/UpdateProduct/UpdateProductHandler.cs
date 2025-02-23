using MediatR;
using OrderManagement.Api.Application.Exceptions;
using OrderManagement.Api.Domain.Entities;
using OrderManagement.Api.Infrastructure.Database;

namespace OrderManagement.Api.Application.Products.UpdateProduct;

public class UpdateProductHandler(OrderManagementDbContext context) : IRequestHandler<UpdateProductCommand, Guid>
{
    public async Task<Guid> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = context.Find<Product>(new ProductId(request.Id));
        
        if(product == null)
            throw new EntityNotFoundException(typeof(Product), request.Id);

        product.Name = request.Name;
        product.Price = request.Price;
        
        context.Products.Update(product);
        await context.SaveChangesAsync(cancellationToken);
        
        return product.Id.Value;
    }
}