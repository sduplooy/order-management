using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace OrderManagement.Api.Infrastructure.Database.ValueConverters;

public static class ModelBuilderExtensions
{
    public static ModelBuilder UseValueConverter(this ModelBuilder modelBuilder, ValueConverter converter)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var properties = entityType.ClrType
                .GetProperties()
                .Where(p => p.PropertyType == converter.ModelClrType);

            foreach (var property in properties)
            {
                modelBuilder
                    .Entity(entityType.Name)
                    .Property(property.Name)
                    .HasConversion(converter);
            }
        }

        return modelBuilder;
    }
}
