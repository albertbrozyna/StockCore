using System.Text.Json;
using System.Text.Json.Serialization;
using Warehouse.Api.Domain.SharedKernel.Abstractions;

namespace Warehouse.Api.Shared.Json;

/// <summary>Serializes a Guid-based strongly-typed ID as a plain GUID string.</summary>
public sealed class StronglyTypedIdJsonConverter<TId> : JsonConverter<TId>
    where TId : struct, IStronglyTypedId<Guid>
{
    public override TId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
        Create(reader.GetGuid());

    public override void Write(Utf8JsonWriter writer, TId value, JsonSerializerOptions options) =>
        writer.WriteStringValue(value.Value);

    public override TId ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
        Create(Guid.Parse(reader.GetString() ?? throw new JsonException("Property name is null.")));

    public override void WriteAsPropertyName(Utf8JsonWriter writer, TId value, JsonSerializerOptions options) =>
        writer.WritePropertyName(value.Value.ToString());

    private static TId Create(Guid value) => (TId)Activator.CreateInstance(typeof(TId), value)!;
}
