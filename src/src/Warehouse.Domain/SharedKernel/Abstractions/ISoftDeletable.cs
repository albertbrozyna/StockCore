using Warehouse.Domain.SharedKernel.Ids;

namespace Warehouse.Domain.SharedKernel.Abstractions;

/// <summary>An object that is deactivated logically instead of being removed.</summary>
public interface ISoftDeletable
{
    bool IsDeleted { get; }

    DateTimeOffset? DeletedAt { get; }

    UserId? DeletedBy { get; }
}
