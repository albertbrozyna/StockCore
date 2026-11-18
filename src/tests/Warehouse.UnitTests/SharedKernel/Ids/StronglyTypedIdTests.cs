using System.Text.Json;
using Warehouse.Api.Domain.SharedKernel.Abstractions;
using Warehouse.Api.Domain.SharedKernel.Ids;
using Warehouse.Api.Infrastructure.Persistence.Conventions;
using Warehouse.Api.Shared.Json;

namespace Warehouse.UnitTests.SharedKernel.Ids;

public sealed class StronglyTypedIdTests
{
    private static readonly JsonSerializerOptions Json = new() { Converters = { new StronglyTypedIdJsonConverterFactory() } };

    private static Type[] AllIdTypes() =>
        typeof(SkuId).Assembly.GetTypes()
            .Where(t => t.IsValueType && typeof(IStronglyTypedId<Guid>).IsAssignableFrom(t))
            .ToArray();

    [Fact]
    public void All_29_ids_from_the_domain_model_exist()
    {
        AllIdTypes().Length.ShouldBe(29);
    }

    [Fact]
    public void New_generates_unique_non_empty_version7_ids()
    {
        var a = SkuId.New();
        var b = SkuId.New();

        a.ShouldNotBe(b);
        a.IsEmpty.ShouldBeFalse();
        a.Value.Version.ShouldBe(7);
    }

    [Fact]
    public void Empty_is_empty_and_ids_of_different_types_are_distinct_types()
    {
        SkuId.Empty.IsEmpty.ShouldBeTrue();
        SkuId.Empty.Value.ShouldBe(Guid.Empty);
        typeof(SkuId).ShouldNotBe(typeof(LotId));
    }

    [Fact]
    public void ToString_returns_the_guid()
    {
        var id = TenantId.New();

        id.ToString().ShouldBe(id.Value.ToString());
    }

    [Fact]
    public void Json_roundtrips_as_plain_guid_string()
    {
        var id = DocumentId.New();

        var json = JsonSerializer.Serialize(id, Json);

        json.ShouldBe($"\"{id.Value}\"");
        JsonSerializer.Deserialize<DocumentId>(json, Json).ShouldBe(id);
    }

    [Fact]
    public void Json_supports_ids_as_dictionary_keys_and_properties()
    {
        var id = SkuId.New();
        var dict = new Dictionary<SkuId, int> { [id] = 5 };

        var json = JsonSerializer.Serialize(dict, Json);

        JsonSerializer.Deserialize<Dictionary<SkuId, int>>(json, Json)![id].ShouldBe(5);
    }

    [Fact]
    public void Ef_converter_roundtrips_every_id_type()
    {
        foreach (var type in AllIdTypes())
        {
            var converter = (Microsoft.EntityFrameworkCore.Storage.ValueConversion.ValueConverter)
                Activator.CreateInstance(typeof(StronglyTypedIdValueConverter<>).MakeGenericType(type))!;
            var guid = Guid.CreateVersion7();
            var id = Activator.CreateInstance(type, guid)!;

            converter.ConvertToProvider(id).ShouldBe(guid);
            converter.ConvertFromProvider(guid).ShouldBe(id);
        }
    }
}
