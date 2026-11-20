using Warehouse.Domain.SharedKernel.Ids;

namespace Warehouse.Domain.SharedKernel.Abstractions;

/// <summary>Creation audit data, stamped by the persistence layer.</summary>
public interface ICreationAudited
{
    DateTimeOffset CreatedAt { get; }

    UserId? CreatedBy { get; }
}
