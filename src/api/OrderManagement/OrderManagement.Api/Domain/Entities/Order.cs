using OrderManagement.Api.Domain.ValueObjects;

namespace OrderManagement.Api.Domain.Entities;

[StronglyTypedId(jsonConverter: StronglyTypedIdJsonConverter.SystemTextJson)]
public partial struct OrderId;

public partial class Order
{
    public OrderId Id { get; set; }
    public IEnumerable<OrderLineItem> LineItems { get; } = [];
}
