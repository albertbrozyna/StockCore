using Warehouse.Domain.Documents.Events;
using Warehouse.Domain.SharedKernel;
using Warehouse.Domain.SharedKernel.Abstractions;
using Warehouse.Domain.SharedKernel.Ids;
using Warehouse.Domain.SharedKernel.ValueObjects;

namespace Warehouse.UnitTests.SharedKernel.Events;

public sealed class DomainEventsTests
{
    private static readonly string[] NonIntegration = ["DocumentCreated", "StockChanged", "PutawayCompleted"];

    private static Type[] AllEvents() =>
        typeof(DomainEvent).Assembly.GetTypes()
            .Where(t => t is { IsAbstract: false, Namespace: not null } && t.Namespace.EndsWith(".Events", StringComparison.Ordinal) && typeof(IDomainEvent).IsAssignableFrom(t))
            .ToArray();

    [Fact]
    public void All_28_events_from_the_catalog_exist()
    {
        AllEvents().Length.ShouldBe(28);
    }

    [Fact]
    public void Integration_marker_matches_the_catalog()
    {
        foreach (var type in AllEvents())
        {
            typeof(IIntegrationEvent).IsAssignableFrom(type).ShouldBe(!NonIntegration.Contains(type.Name), type.Name);
        }
    }

    [Fact]
    public void Events_do_not_expose_value_objects()
    {
        var voNamespace = typeof(Money).Namespace;
        foreach (var property in AllEvents().SelectMany(t => t.GetProperties()))
        {
            property.PropertyType.Namespace.ShouldNotBe(voNamespace, $"{property.DeclaringType!.Name}.{property.Name}");
        }
    }

    [Fact]
    public void Event_carries_tenant_time_and_unique_id()
    {
        var tenant = TenantId.New();
        var at = DateTimeOffset.UnixEpoch;
        var a = new DocumentCancelled(tenant, at, DocumentId.New(), "x");
        var b = new DocumentCancelled(tenant, at, DocumentId.New(), "x");

        a.TenantId.ShouldBe(tenant);
        a.OccurredAt.ShouldBe(at);
        a.EventId.ShouldNotBe(b.EventId);
    }
}
