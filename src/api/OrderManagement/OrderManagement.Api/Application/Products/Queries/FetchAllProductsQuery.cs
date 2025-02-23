using MediatR;
using OrderManagement.Api.Domain.Entities;

namespace OrderManagement.Api.Application.Products.Queries;

public record FetchAllProductsQuery : IRequest<IEnumerable<Product>>;
