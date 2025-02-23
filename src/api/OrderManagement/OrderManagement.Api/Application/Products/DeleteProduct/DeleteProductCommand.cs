using MediatR;
using OrderManagement.Api.Domain.Entities;

namespace OrderManagement.Api.Application.Products.DeleteProduct;

public record DeleteProductCommand(ProductId ProductId) : IRequest;
