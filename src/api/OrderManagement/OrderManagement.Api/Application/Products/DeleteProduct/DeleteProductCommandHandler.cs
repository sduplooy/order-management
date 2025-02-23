using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderManagement.Api.Infrastructure.Database;

namespace OrderManagement.Api.Application.Products.DeleteProduct;

public class DeleteProductCommandHandler(OrderManagementDbContext context) : IRequestHandler<DeleteProductCommand>
{
    public async Task Handle(DeleteProductCommand command, CancellationToken cancellationToken)
    {
        var product = await context.Products.SingleOrDefaultAsync(p => p.Id == command.ProductId, cancellationToken);

        if (product == null)
            return;
        
        context.Products.Remove(product);
        await context.SaveChangesAsync(cancellationToken);
    }
}
