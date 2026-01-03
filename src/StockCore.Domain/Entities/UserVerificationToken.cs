using StockCore.Domain.Enums;
using StockCore.Domain.Interfaces;
using System;

namespace StockCore.Domain.Entities;

public class UserVerificationToken :ITenantEntity
{
    public Guid Id { get; private set; }

    public Guid UserId { get; private set; }
    public User User { get; private set; } = null!;

    public string TokenHash { get; private set; } = null!;
    public UserVerificationTokenType Type { get; private set; }

    public DateTime CreatedAt { get; private set; }
    public DateTime ExpiresAt { get; private set; }
    public DateTime? UsedAt { get; private set; }

    public int FailedAttempts { get; private set; }

    public string? CreatedFromIp { get; private set; }
    public string? UsedFromIp { get; private set; }
    public string? UserAgent { get; private set; }
    public Guid TenantId { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    protected UserVerificationToken() { }

    public UserVerificationToken(
        Guid userId,
        string tokenHash,
        UserVerificationTokenType type,
        DateTime expiresAt,
        Guid tenantId,
        string? createdFromIp = null,
        string? userAgent = null)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        TokenHash = tokenHash;
        Type = type;
        CreatedAt = DateTime.UtcNow;
        ExpiresAt = expiresAt;
        TenantId = tenantId;
        CreatedFromIp = createdFromIp;
        UserAgent = userAgent;
    }

    public bool IsExpired()
        => DateTime.UtcNow > ExpiresAt;

    public bool IsUsed()
        => UsedAt.HasValue;

    public bool IsValid()
        => !IsExpired() && !IsUsed();

    public void MarkAsUsed(string? usedFromIp = null)
    {
        UsedAt = DateTime.UtcNow;
        UsedFromIp = usedFromIp;
    }

    public void RegisterFailedAttempt()
    {
        FailedAttempts++;
    }
}
