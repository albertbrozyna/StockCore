using Warehouse.Api.Domain.SharedKernel;
using Warehouse.Api.Domain.SharedKernel.Enums;
using Warehouse.Api.Domain.SharedKernel.ValueObjects;
using Warehouse.Api.Domain.Tenancy;
using Warehouse.Api.Domain.Tenancy.Events;

namespace Warehouse.UnitTests.Domain;

public sealed class TenancyTests
{
    private static readonly DateTimeOffset Now = new(2026, 1, 1, 8, 0, 0, TimeSpan.Zero);

    private static Tenant NewTenant() =>
        Tenant.Create(TenantCode.Create("acme"), "Acme", "Europe/Warsaw", TenantSettings.Default, Now);

    [Fact]
    public void Create_activates_tenant_and_raises_event()
    {
        var tenant = NewTenant();

        tenant.Status.ShouldBe(TenantStatus.Active);
        tenant.ActivatedAt.ShouldBe(Now);
        tenant.DomainEvents.OfType<TenantCreated>().Single().TenantId.ShouldBe(tenant.Id);
    }

    [Fact]
    public void Create_rejects_unknown_time_zone()
    {
        var ex = Should.Throw<DomainException>(() =>
            Tenant.Create(TenantCode.Create("acme"), "Acme", "Mars/Olympus", TenantSettings.Default, Now));
        ex.Error.Code.ShouldBe("Tenancy.InvalidTimeZone");
    }

    [Fact]
    public void Suspend_then_activate_round_trips_and_raises_event()
    {
        var tenant = NewTenant();
        tenant.Suspend("unpaid", Now.AddDays(1));

        tenant.Status.ShouldBe(TenantStatus.Suspended);
        tenant.SuspendedAt.ShouldBe(Now.AddDays(1));
        tenant.DomainEvents.OfType<TenantSuspended>().Count().ShouldBe(1);

        tenant.Activate(Now.AddDays(2));
        tenant.Status.ShouldBe(TenantStatus.Active);
        tenant.SuspendedAt.ShouldBeNull();
    }

    [Fact]
    public void Suspend_requires_active_tenant_and_reason()
    {
        var tenant = NewTenant();
        Should.Throw<DomainException>(() => tenant.Suspend(" ", Now));
        tenant.Deactivate();
        Should.Throw<DomainException>(() => tenant.Suspend("x", Now)).Error.Code.ShouldBe("Tenancy.NotActive");
    }

    [Fact]
    public void UpdateSettings_validates_padding()
    {
        var bad = TenantSettings.Default with { Numbering = new DocumentNumberingPolicy(true, true, 0, null) };
        Should.Throw<DomainException>(() => NewTenant().UpdateSettings(bad));
    }

    [Fact]
    public void Sequence_next_increments_from_zero()
    {
        var sequence = DocumentNumberSequence.Create(DocumentType.Receipt, "PZ", year: 2026);

        sequence.Next().ShouldBe(1);
        sequence.Next().ShouldBe(2);
        sequence.LastValue.ShouldBe(2);
    }
}
