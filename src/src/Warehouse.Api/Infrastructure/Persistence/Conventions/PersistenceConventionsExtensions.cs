using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Warehouse.Api.Domain.SharedKernel;
using Warehouse.Api.Domain.SharedKernel.Enums;

namespace Warehouse.Api.Infrastructure.Persistence.Conventions;

public static class PersistenceConventionsExtensions
{
    /// <summary>decimal(18,4), strongly-typed ID converters, enums stored as strings, smart enums stored by name.</summary>
    public static ModelConfigurationBuilder ConfigurePersistenceConventions(this ModelConfigurationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.Properties<decimal>().HavePrecision(18, 4);
        builder.ConfigureStronglyTypedIds();

        var domainTypes = typeof(Enumeration).Assembly.GetTypes();
        foreach (var enumType in domainTypes.Where(t => t.IsEnum && t.Namespace == typeof(BarcodeType).Namespace))
        {
            var converter = typeof(EnumToStringConverter<>).MakeGenericType(enumType);
            builder.Properties(enumType).HaveConversion(converter);
            builder.Properties(typeof(Nullable<>).MakeGenericType(enumType)).HaveConversion(converter);
        }

        foreach (var smartEnum in domainTypes.Where(t => t is { IsAbstract: false } && typeof(Enumeration).IsAssignableFrom(t)))
        {
            builder.Properties(smartEnum).HaveConversion(typeof(EnumerationValueConverter<>).MakeGenericType(smartEnum));
        }

        return builder;
    }
}
