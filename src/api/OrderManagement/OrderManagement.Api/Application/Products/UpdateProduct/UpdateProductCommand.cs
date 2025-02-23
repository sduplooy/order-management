using MediatR;

namespace OrderManagement.Api.Application.Products.UpdateProduct;

public record UpdateProductCommand(Guid Id, string Name, decimal Price) : IRequest<Guid>;