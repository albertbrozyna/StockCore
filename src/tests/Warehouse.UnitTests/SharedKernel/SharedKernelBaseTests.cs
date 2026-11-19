using Warehouse.Domain.SharedKernel;
using Warehouse.Domain.SharedKernel.Abstractions;
using Warehouse.Domain.SharedKernel.Ids;

namespace Warehouse.UnitTests.SharedKernel;

public sealed class SharedKernelBaseTests
{
    private sealed record TestEvent(TenantId TenantId, DateTimeOffset OccurredAt) : DomainEvent(TenantId, OccurredAt);

    private sealed class TestEntity : SoftDeletableTenantEntity<SkuId>
    {
        public TestEntity(SkuId id)
            : base(id)
        {
        }

        public void DoSomething() => Raise(new TestEvent(TenantId, DateTimeOffset.UnixEpoch));
    }

    private sealed class OtherEntity(SkuId id) : Entity<SkuId>(id);

    private sealed class Money(decimal amount, string currency) : ValueObject
    {
        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return amount;
            yield return currency;
        }
    }

    private sealed class Ledger(StockMovementId id) : ImmutableTenantEntity<StockMovementId>(id);

    [Fact]
    public void Entities_with_same_id_and_type_are_equal()
    {
        var id = SkuId.New();

        (new TestEntity(id) == new TestEntity(id)).ShouldBeTrue();
        new TestEntity(id).GetHashCode().ShouldBe(new TestEntity(id).GetHashCode());
        new TestEntity(id).Equals(new OtherEntity(id)).ShouldBeFalse();
        (new TestEntity(SkuId.New()) != new TestEntity(SkuId.New())).ShouldBeTrue();
    }

    [Fact]
    public void Transient_entities_are_equal_only_by_reference()
    {
        var a = new TestEntity(SkuId.Empty);

        a.Equals(new TestEntity(SkuId.Empty)).ShouldBeFalse();
        a.Equals(a).ShouldBeTrue();
    }

    [Fact]
    public void Domain_events_are_raised_and_cleared()
    {
        var entity = new TestEntity(SkuId.New());

        entity.DoSomething();

        entity.DomainEvents.Count.ShouldBe(1);
        entity.DomainEvents.Single().EventId.ShouldNotBe(Guid.Empty);
        entity.ClearDomainEvents();
        entity.DomainEvents.ShouldBeEmpty();
    }

    [Fact]
    public void Tenant_can_be_set_once_and_never_changed()
    {
        var entity = new TestEntity(SkuId.New());
        var tenant = TenantId.New();

        entity.SetTenant(tenant);
        entity.SetTenant(tenant);

        entity.TenantId.ShouldBe(tenant);
        var ex = Should.Throw<DomainException>(() => entity.SetTenant(TenantId.New()));
        ex.Error.Code.ShouldBe("SharedKernel.TenantImmutable");
    }

    [Fact]
    public void Immutable_entity_tenant_and_creation_are_stamped_once()
    {
        var entry = new Ledger(StockMovementId.New());
        var tenant = TenantId.New();
        var at = DateTimeOffset.UnixEpoch.AddDays(1);

        entry.SetTenant(tenant);
        entry.StampCreated(at, null);

        entry.TenantId.ShouldBe(tenant);
        entry.CreatedAt.ShouldBe(at);
        Should.Throw<DomainException>(() => entry.SetTenant(TenantId.New()));
    }

    [Fact]
    public void Audit_stamps_are_applied()
    {
        var entity = new TestEntity(SkuId.New());
        var user = UserId.New();
        var at = DateTimeOffset.UnixEpoch.AddDays(2);

        entity.StampCreated(at, user);
        entity.StampModified(at.AddHours(1), user);

        entity.CreatedAt.ShouldBe(at);
        entity.CreatedBy.ShouldBe(user);
        entity.ModifiedAt.ShouldBe(at.AddHours(1));
        entity.ModifiedBy.ShouldBe(user);
    }

    [Fact]
    public void Soft_delete_and_restore_toggle_state()
    {
        var entity = new TestEntity(SkuId.New());

        entity.Delete();
        entity.StampDeleted(DateTimeOffset.UnixEpoch, UserId.New());
        entity.IsDeleted.ShouldBeTrue();
        entity.DeletedAt.ShouldNotBeNull();

        entity.Restore();
        entity.IsDeleted.ShouldBeFalse();
        entity.DeletedAt.ShouldBeNull();
        entity.DeletedBy.ShouldBeNull();
    }

    [Fact]
    public void Value_objects_compare_by_components()
    {
        (new Money(1m, "PLN") == new Money(1m, "PLN")).ShouldBeTrue();
        (new Money(1m, "PLN") != new Money(2m, "PLN")).ShouldBeTrue();
        new Money(1m, "PLN").GetHashCode().ShouldBe(new Money(1m, "PLN").GetHashCode());
    }

    [Fact]
    public void Result_reports_success_and_failure()
    {
        var error = new Error("Inventory.InsufficientStock", "no stock", ErrorType.Conflict);

        Result.Success().IsSuccess.ShouldBeTrue();
        default(Result).IsSuccess.ShouldBeTrue();
        Result.Failure(error).Error.ShouldBe(error);
        Result.Success(5).Value.ShouldBe(5);
        var failed = Result.Failure<int>(error);
        failed.IsFailure.ShouldBeTrue();
        Should.Throw<InvalidOperationException>(() => failed.Value);
    }

    [Fact]
    public void Guard_rejects_invalid_arguments()
    {
        Should.Throw<DomainException>(() => Guard.AgainstNull<string>(null));
        Should.Throw<DomainException>(() => Guard.AgainstNullOrWhiteSpace(" "));
        Should.Throw<DomainException>(() => Guard.AgainstOutOfRange(11, 1, 10));
        Should.Throw<DomainException>(() => Guard.AgainstNegative(-1m));
        Should.Throw<DomainException>(() => Guard.AgainstDefault(SkuId.Empty));

        Guard.AgainstOutOfRange(5, 1, 10).ShouldBe(5);
        Guard.AgainstNegative(0).ShouldBe(0);
        Guard.AgainstNullOrWhiteSpace("x").ShouldBe("x");
    }

    [Fact]
    public void Guard_errors_are_validation_errors_with_parameter_name()
    {
        var ex = Should.Throw<DomainException>(() => Guard.AgainstNullOrWhiteSpace(string.Empty));

        ex.Error.Type.ShouldBe(ErrorType.Validation);
        ex.Error.Message.ShouldContain("string.Empty");
    }
}
