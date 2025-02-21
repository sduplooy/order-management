namespace OrderManagement.Api.Domain.Entities;

[StronglyTypedId(jsonConverter: StronglyTypedIdJsonConverter.SystemTextJson)]
public partial struct ProductId;

public partial class Product
{
    public ProductId Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
}
