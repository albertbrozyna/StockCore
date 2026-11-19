using Microsoft.EntityFrameworkCore;
using Warehouse.Api.Application.Abstractions;
using Warehouse.Api.Domain.Catalog.Events;
using Warehouse.Api.Domain.SharedKernel;
using Warehouse.Api.Domain.SharedKernel.Enums;
using Warehouse.Api.Domain.SharedKernel.Ids;
using Warehouse.Api.Infrastructure.Persistence;
using Warehouse.Api.Infrastructure.Persistence.Conventions;
using Warehouse.Api.Infrastructure.Persistence.Interceptors;
using Warehouse.Api.Infrastructure.Persistence.Outbox;

namespace Warehouse.UnitTests.Infrastructure;

/// <summary>Model and interceptor logic checked without a database: the Npgsql provider builds models and SQL without connecting.</summary>
public sealed class PersistenceModelTests
{
    private sealed class FakeTenant(TenantId id, bool resolved = true) : ITenantContext
    {
        public TenantId TenantId { get; } = id;

        public bool IsResolved { get; } = resolved;
    }

    private sealed class FakeClock : IClock
    {
        public static readonly DateTimeOffset Now = new(2026, 5, 1, 12, 0, 0, TimeSpan.Zero);

        public DateTimeOffset UtcNow => Now;

        public DateOnly Today(string timeZone) => DateOnly.FromDateTime(Now.DateTime);
    }

    private sealed class FakeUser(UserId? id) : ICurrentUser
    {
        public UserId? UserId { get; } = id;

        public string? Name => "test";

        public IReadOnlySet<string> Roles { get; } = new HashSet<string>();

        public bool HasPermission(string permission) => true;
    }

    private sealed class Product : SoftDeletableTenantEntity<SkuId>
    {
        private Product()
        {
        }

        public Product(SkuId id)
            : base(id)
        {
        }

        public decimal Price { get; set; }

        public LengthUnit? Unit { get; set; }

        public DocumentStatus Status { get; set; } = DocumentStatus.Draft;

        public void Publish(TenantId tenant) => Raise(new SkuCreated(tenant, FakeClock.Now, Id, "X"));
    }

    private sealed class Entry : ImmutableTenantEntity<StockMovementId>
    {
        private Entry()
        {
        }

        public Entry(StockMovementId id)
            : base(id)
        {
        }
    }

    private sealed class TestDbContext(DbContextOptions<TestDbContext> options, ITenantContext tenant) : DbContext(options), ITenantFilterSource
    {
        public TenantId CurrentTenantId => tenant.IsResolved ? tenant.TenantId : default;

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder) =>
            configurationBuilder.ConfigurePersistenceConventions();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().ToTable("products", "catalog");
            modelBuilder.Entity<Entry>().ToTable("entries", "inventory");
            modelBuilder.Entity<OutboxMessage>().ToTable("outbox_messages", "platform");
            modelBuilder.ApplyDomainConventions();
            modelBuilder.ApplyTenantAndSoftDeleteFilters(this);
        }
    }

    private static TestDbContext NewContext(ITenantContext tenant) =>
        new(new DbContextOptionsBuilder<TestDbContext>().UseNpgsql("Host=localhost;Database=none").Options, tenant);

    [Fact]
    public void Conventions_map_decimal_enums_smart_enums_and_version()
    {
        using var db = NewContext(new FakeTenant(TenantId.New()));
        var product = db.Model.FindEntityType(typeof(Product))!;

        product.FindProperty(nameof(Product.Price))!.GetPrecision().ShouldBe(18);
        product.FindProperty(nameof(Product.Price))!.GetScale().ShouldBe(4);
        product.FindProperty(nameof(Product.Unit))!.GetValueConverter()!.ProviderClrType.ShouldBe(typeof(string));
        product.FindProperty(nameof(Product.Status))!.GetValueConverter()!.ProviderClrType.ShouldBe(typeof(string));
        product.FindProperty(nameof(Product.Id))!.GetValueConverter()!.ProviderClrType.ShouldBe(typeof(Guid));
        product.FindProperty(nameof(Product.Version))!.IsConcurrencyToken.ShouldBeTrue();
        product.FindProperty(nameof(Product.Version))!.GetColumnName().ShouldBe("xmin");
        product.FindProperty(nameof(Product.DomainEvents)).ShouldBeNull();
        product.GetSchema().ShouldBe("catalog");
    }

    [Fact]
    public void Outbox_message_is_configured_in_the_platform_schema_by_folder_convention()
    {
        using var db = new WarehouseDbContext(
            new DbContextOptionsBuilder<WarehouseDbContext>().UseNpgsql("Host=localhost;Database=none").Options,
            new FakeTenant(TenantId.New()));
        var outbox = db.Model.FindEntityType(typeof(OutboxMessage))!;

        outbox.GetSchema().ShouldBe("platform");
        outbox.GetTableName().ShouldBe("outbox_messages");
        outbox.FindProperty(nameof(OutboxMessage.Payload))!.GetColumnType().ShouldBe("jsonb");
    }

    [Fact]
    public void Query_filter_is_evaluated_per_context_tenant()
    {
        var a = TenantId.New();
        var b = TenantId.New();
        using var dbA = NewContext(new FakeTenant(a));
        using var dbB = NewContext(new FakeTenant(b));

        var sqlA = dbA.Set<Product>().ToQueryString();
        var sqlB = dbB.Set<Product>().ToQueryString();

        sqlA.ShouldContain(a.Value.ToString());
        sqlB.ShouldContain(b.Value.ToString());
        sqlB.ShouldNotContain(a.Value.ToString());
        sqlB.ShouldContain("IsDeleted");
    }

    [Fact]
    public void Auditing_stamps_creation_modification_and_tenant()
    {
        var tenant = TenantId.New();
        var user = UserId.New();
        var auditing = new AuditingInterceptor(new FakeClock(), new FakeUser(user), new FakeTenant(tenant));
        using var db = NewContext(new FakeTenant(tenant));
        var product = new Product(SkuId.New());
        db.Add(product);

        auditing.Apply(db);

        product.TenantId.ShouldBe(tenant);
        product.CreatedAt.ShouldBe(FakeClock.Now);
        product.CreatedBy.ShouldBe(user);

        db.Entry(product).State = EntityState.Unchanged;
        product.Price = 5m;
        db.Entry(product).State = EntityState.Modified;
        auditing.Apply(db);

        product.ModifiedAt.ShouldBe(FakeClock.Now);
        product.ModifiedBy.ShouldBe(user);
    }

    [Fact]
    public void Tenant_guard_rejects_foreign_or_unresolved_tenant_writes()
    {
        var mine = TenantId.New();
        using var db = NewContext(new FakeTenant(mine));
        var product = new Product(SkuId.New());
        ((ITenantStampAccessor)new TenantStampAccessor()).Set(product, TenantId.New());
        db.Add(product);

        Should.Throw<DomainException>(() => new TenantSaveChangesGuard(new FakeTenant(mine)).Check(db))
            .Error.Code.ShouldBe("Tenancy.CrossTenantWrite");
        Should.Throw<DomainException>(() => new TenantSaveChangesGuard(new FakeTenant(default, resolved: false)).Check(db));
    }

    [Fact]
    public void Tenant_guard_allows_own_tenant()
    {
        var mine = TenantId.New();
        using var db = NewContext(new FakeTenant(mine));
        var product = new Product(SkuId.New());
        new AuditingInterceptor(new FakeClock(), new FakeUser(null), new FakeTenant(mine)).Apply(db);
        db.Add(product);
        new AuditingInterceptor(new FakeClock(), new FakeUser(null), new FakeTenant(mine)).Apply(db);

        Should.NotThrow(() => new TenantSaveChangesGuard(new FakeTenant(mine)).Check(db));
    }

    [Fact]
    public void Immutable_guard_rejects_update_and_delete_but_allows_insert()
    {
        using var db = NewContext(new FakeTenant(TenantId.New()));
        var entry = new Entry(StockMovementId.New());
        db.Add(entry);
        Should.NotThrow(() => ImmutableEntityGuard.Check(db));

        db.Entry(entry).State = EntityState.Unchanged;
        db.Entry(entry).State = EntityState.Modified;
        Should.Throw<DomainException>(() => ImmutableEntityGuard.Check(db)).Error.Code.ShouldBe("Persistence.ImmutableEntity");

        db.Entry(entry).State = EntityState.Deleted;
        Should.Throw<DomainException>(() => ImmutableEntityGuard.Check(db));
    }

    [Fact]
    public void Soft_delete_converts_delete_to_update_and_stamps()
    {
        var user = UserId.New();
        using var db = NewContext(new FakeTenant(TenantId.New()));
        var product = new Product(SkuId.New());
        db.Attach(product);
        db.Remove(product);

        new SoftDeleteInterceptor(new FakeClock(), new FakeUser(user)).Apply(db);

        db.Entry(product).State.ShouldBe(EntityState.Modified);
        product.IsDeleted.ShouldBeTrue();
        product.DeletedAt.ShouldBe(FakeClock.Now);
        product.DeletedBy.ShouldBe(user);
    }

    [Fact]
    public void Outbox_interceptor_copies_only_integration_events()
    {
        var tenant = TenantId.New();
        using var db = NewContext(new FakeTenant(tenant));
        var product = new Product(SkuId.New());
        product.Publish(tenant);
        db.Add(product);

        OutboxInterceptor.Apply(db);

        var message = db.ChangeTracker.Entries<OutboxMessage>().Single().Entity;
        message.Type.ShouldBe(typeof(SkuCreated).FullName);
        message.TenantId.ShouldBe(tenant);
        message.Payload.ShouldContain("\"code\":\"X\"");
    }

    [Fact]
    public async Task Domain_events_are_collected_cleared_and_dispatched_after_save()
    {
        var tenant = TenantId.New();
        var dispatcher = new RecordingDispatcher();
        var interceptor = new DomainEventsInterceptor(dispatcher);
        using var db = NewContext(new FakeTenant(tenant));
        var product = new Product(SkuId.New());
        product.Publish(tenant);
        db.Add(product);

        interceptor.Collect(db);
        product.DomainEvents.ShouldBeEmpty();
        dispatcher.Dispatched.ShouldBeEmpty();

        await interceptor.SavedChangesAsync(null!, 1);
        dispatcher.Dispatched.Count.ShouldBe(1);
    }

    [Fact]
    public void Outbox_payload_serializes_smart_enums_by_name_and_ids_as_guids()
    {
        var tenant = TenantId.New();
        var documentId = DocumentId.New();
        var approved = new Warehouse.Api.Domain.Documents.Events.DocumentApproved(
            tenant, FakeClock.Now, documentId, DocumentType.Receipt, "PZ/WH1/2026/000001", WarehouseId.New(), FakeClock.Now);

        var message = OutboxMessage.Create(approved);

        message.Payload.ShouldContain("\"type\":\"Receipt\"");
        message.Payload.ShouldContain($"\"documentId\":\"{documentId}\"");
        message.Id.ShouldBe(approved.EventId);
    }

    private sealed class RecordingDispatcher : IDomainEventDispatcher
    {
        public List<Warehouse.Api.Domain.SharedKernel.Abstractions.IDomainEvent> Dispatched { get; } = [];

        public Task DispatchAsync(IEnumerable<Warehouse.Api.Domain.SharedKernel.Abstractions.IDomainEvent> events, CancellationToken ct)
        {
            Dispatched.AddRange(events);
            return Task.CompletedTask;
        }
    }

    private interface ITenantStampAccessor
    {
        void Set(object entity, TenantId tenant);
    }

    private sealed class TenantStampAccessor : ITenantStampAccessor
    {
        public void Set(object entity, TenantId tenant) => ((Product)entity).SetTenant(tenant);
    }
}
