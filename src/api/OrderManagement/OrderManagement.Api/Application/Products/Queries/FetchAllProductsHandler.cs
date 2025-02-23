using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderManagement.Api.Domain.Entities;
using OrderManagement.Api.Infrastructure.Database;

namespace OrderManagement.Api.Application.Products.Queries;

public class FetchAllProductsHandler(OrderManagementDbContext context) : IRequestHandler<FetchAllProductsQuery, IEnumerable<Product>>
{
    public Task<IEnumerable<Product>> Handle(FetchAllProductsQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult<IEnumerable<Product>>(context.Products.AsNoTracking().ToList());
    }
}