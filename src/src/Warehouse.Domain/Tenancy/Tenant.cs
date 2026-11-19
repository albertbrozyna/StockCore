using Warehouse.Domain.SharedKernel;
using Warehouse.Domain.SharedKernel.Abstractions;
using Warehouse.Domain.SharedKernel.Enums;
using Warehouse.Domain.SharedKernel.Ids;
using Warehouse.Domain.SharedKernel.ValueObjects;
using Warehouse.Domain.Tenancy.Events;

namespace Warehouse.Domain.Tenancy;

/// <summary>A company using the system. All other data is partitioned by its id.</summary>
public sealed class Tenant : AuditableEntity<TenantId>, IAggregateRoot
{
    private Tenant()
    {
        Code = null!;
        Name = null!;
        TimeZone = null!;
        Settings = null!;
    }

    public TenantCode Code { get; private set; }

    public string Name { get; private set; }

    public TenantStatus Status { get; private set; }

    public string TimeZone { get; private set; }

    public TenantSettings Settings { get; private set; }

    public TaxId? TaxId { get; private set; }

    public Address? Address { get; private set; }

    public DateTimeOffset? ActivatedAt { get; private set; }

    public DateTimeOffset? SuspendedAt { get; private set; }

    public static Tenant Create(
        TenantCode code,
        string name,
        string timeZone,
        TenantSettings settings,
        DateTimeOffset now,
        TaxId? taxId = null,
        Address? address = null)
    {
        var tenant = new Tenant
        {
            Id = TenantId.New(),
            Code = Guard.AgainstNull(code),
            Name = ValidateName(name),
            TimeZone = ValidateTimeZone(timeZone),
            Settings = ValidateSettings(settings),
            TaxId = taxId,
            Address = address,
            Status = TenantStatus.Active,
            ActivatedAt = now,
        };
        tenant.Raise(new TenantCreated(tenant.Id, now));
        return tenant;
    }

    public void Rename(string name) => Name = ValidateName(name);

    public void UpdateSettings(TenantSettings settings) => Settings = ValidateSettings(settings);

    /// <summary>Suspends an active tenant. The reason is mandatory for the audit trail but is not stored on the aggregate.</summary>
    public void Suspend(string reason, DateTimeOffset now)
    {
        Guard.AgainstNullOrWhiteSpace(reason);
        if (Status != TenantStatus.Active)
        {
            throw Conflict("Tenancy.NotActive", "Only an active tenant can be suspended.");
        }

        Status = TenantStatus.Suspended;
        SuspendedAt = now;
        Raise(new TenantSuspended(Id, now));
    }

    /// <summary>Reactivates a suspended or deactivated tenant.</summary>
    public void Activate(DateTimeOffset now)
    {
        if (Status == TenantStatus.Active)
        {
            throw Conflict("Tenancy.AlreadyActive", "The tenant is already active.");
        }

        Status = TenantStatus.Active;
        ActivatedAt = now;
        SuspendedAt = null;
    }

    public void Deactivate()
    {
        if (Status == TenantStatus.Deactivated)
        {
            throw Conflict("Tenancy.AlreadyDeactivated", "The tenant is already deactivated.");
        }

        Status = TenantStatus.Deactivated;
    }

    private static string ValidateName(string name)
    {
        var trimmed = Guard.AgainstNullOrWhiteSpace(name).Trim();
        return trimmed.Length > 200
            ? throw Invalid("Tenancy.InvalidName", "Tenant name must have at most 200 characters.")
            : trimmed;
    }

    private static string ValidateTimeZone(string timeZone)
    {
        var id = Guard.AgainstNullOrWhiteSpace(timeZone).Trim();
        return TimeZoneInfo.TryFindSystemTimeZoneById(id, out _)
            ? id
            : throw Invalid("Tenancy.InvalidTimeZone", $"'{id}' is not a known IANA time zone.");
    }

    private static TenantSettings ValidateSettings(TenantSettings settings)
    {
        Guard.AgainstNull(settings);
        Guard.AgainstNull(settings.Numbering);
        Guard.AgainstOutOfRange(settings.Numbering.SequencePadding, 1, 18);
        if (settings.DefaultReservationTtl is { } ttl && ttl <= TimeSpan.Zero)
        {
            throw Invalid("Tenancy.InvalidReservationTtl", "Default reservation TTL must be positive.");
        }

        return settings;
    }

    private static DomainException Invalid(string code, string message) =>
        new(new Error(code, message, ErrorType.Validation));

    private static DomainException Conflict(string code, string message) =>
        new(new Error(code, message, ErrorType.Conflict));
}
