using System.Text.Json;
using System.Text.Json.Serialization;
using Warehouse.Api.Shared.Json;

namespace Warehouse.Api.Infrastructure.Persistence.Outbox;

internal static class OutboxJson
{
    private static readonly JsonSerializerOptions Options = new(JsonSerializerDefaults.Web)
    {
        Converters =
        {
            new StronglyTypedIdJsonConverterFactory(),
            new EnumerationJsonConverterFactory(),
            new JsonStringEnumConverter(),
        },
    };

    /// <summary>Serializes using the runtime type so derived event members are included.</summary>
    public static string Serialize(object value) => JsonSerializer.Serialize(value, value.GetType(), Options);
}
