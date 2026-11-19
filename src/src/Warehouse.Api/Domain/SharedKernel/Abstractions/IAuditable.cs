using Warehouse.Api.Domain.SharedKernel.Ids;

namespace Warehouse.Api.Domain.SharedKernel.Abstractions;

/// <summary>Creation and modification audit data, stamped by the persistence layer.</summary>
public interface IAuditable : ICreationAudited
{
    DateTimeOffset? ModifiedAt { get; }

    UserId? ModifiedBy { get; }
}
