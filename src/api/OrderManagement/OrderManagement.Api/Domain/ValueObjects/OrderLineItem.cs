using OrderManagement.Api.Domain.Entities;

namespace OrderManagement.Api.Domain.ValueObjects;

public record OrderLineItem
{
    public OrderId OrderId { get; set; }
    public ProductId ProductId { get; set; }
    public int Quantity { get; set; }
}
