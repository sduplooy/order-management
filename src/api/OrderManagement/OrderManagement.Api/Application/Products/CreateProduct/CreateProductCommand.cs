using MediatR;
using OrderManagement.Api.Domain.Entities;

namespace OrderManagement.Api.Application.Products.CreateProduct;

public record CreateProductCommand(string Name, decimal Price) : IRequest<ProductId>;