using System.Text.Json;
using System.Text.Json.Serialization;
using Warehouse.Api.Domain.SharedKernel;

namespace Warehouse.Api.Shared.Json;

/// <summary>Reads and writes a smart enum as its <see cref="Enumeration.Name"/>.</summary>
public sealed class EnumerationJsonConverter<T> : JsonConverter<T>
    where T : Enumeration
{
    public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
        reader.TokenType == JsonTokenType.Null ? null : Enumeration.FromName<T>(reader.GetString()!);

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(writer);
        ArgumentNullException.ThrowIfNull(value);
        writer.WriteStringValue(value.Name);
    }
}
