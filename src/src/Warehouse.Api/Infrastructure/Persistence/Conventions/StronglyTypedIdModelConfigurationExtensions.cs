using Microsoft.EntityFrameworkCore;
using Warehouse.Api.Domain.SharedKernel.Abstractions;

namespace Warehouse.Api.Infrastructure.Persistence.Conventions;

public static class StronglyTypedIdModelConfigurationExtensions
{
    /// <summary>Registers a value converter for every Guid-based strongly-typed ID found in the domain assembly.</summary>
    public static ModelConfigurationBuilder ConfigureStronglyTypedIds(this ModelConfigurationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        foreach (var idType in StronglyTypedIdScanner.FindIdTypes(typeof(IStronglyTypedId<>).Assembly))
        {
            var converterType = typeof(StronglyTypedIdValueConverter<>).MakeGenericType(idType);
            builder.Properties(idType).HaveConversion(converterType);
        }

        return builder;
    }
}
