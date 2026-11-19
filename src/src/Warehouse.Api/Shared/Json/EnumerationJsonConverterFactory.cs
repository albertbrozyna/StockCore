using System.Text.Json;
using System.Text.Json.Serialization;
using Warehouse.Api.Domain.SharedKernel;

namespace Warehouse.Api.Shared.Json;

/// <summary>Creates <see cref="EnumerationJsonConverter{T}"/> for every concrete smart enum.</summary>
public sealed class EnumerationJsonConverterFactory : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert) =>
        typeToConvert is { IsAbstract: false } && typeof(Enumeration).IsAssignableFrom(typeToConvert);

    public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options) =>
        (JsonConverter?)Activator.CreateInstance(typeof(EnumerationJsonConverter<>).MakeGenericType(typeToConvert));
}
