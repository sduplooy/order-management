using MediatR;
using OrderManagement.Api.Domain.Entities;

namespace OrderManagement.Api.Application.Products.Queries;

public record AllProductsQuery : IRequest<IEnumerable<Product>>;
