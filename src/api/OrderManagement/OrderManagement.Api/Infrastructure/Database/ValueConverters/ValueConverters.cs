using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OrderManagement.Api.Domain.Entities;

namespace OrderManagement.Api.Infrastructure.Database.ValueConverters;

public class ProductIdValueConverter(ConverterMappingHints mappingHints = null!)
    : ValueConverter<ProductId, Guid>(id => id.Value, value => new ProductId(value), mappingHints);

public class OrderIdValueConverter(ConverterMappingHints mappingHints = null!)
    : ValueConverter<OrderId, Guid>(id => id.Value, value => new OrderId(value), mappingHints);
