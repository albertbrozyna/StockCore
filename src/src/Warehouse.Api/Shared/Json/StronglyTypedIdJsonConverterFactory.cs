using System.Text.Json;
using System.Text.Json.Serialization;
using Warehouse.Api.Domain.SharedKernel.Abstractions;

namespace Warehouse.Api.Shared.Json;

public sealed class StronglyTypedIdJsonConverterFactory : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert) =>
        typeToConvert is { IsValueType: true } && typeof(IStronglyTypedId<Guid>).IsAssignableFrom(typeToConvert);

    public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options) =>
        (JsonConverter?)Activator.CreateInstance(typeof(StronglyTypedIdJsonConverter<>).MakeGenericType(typeToConvert));
}
