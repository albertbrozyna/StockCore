using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Warehouse.Api.Domain.SharedKernel;

namespace Warehouse.Api.Infrastructure.Persistence.Conventions;

/// <summary>Stores a smart enum as its <see cref="Enumeration.Name"/>.</summary>
public sealed class EnumerationValueConverter<T> : ValueConverter<T, string>
    where T : Enumeration
{
    public EnumerationValueConverter()
        : base(value => value.Name, name => Enumeration.FromName<T>(name))
    {
    }
}
