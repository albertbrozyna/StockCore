using Warehouse.Domain.SharedKernel.Ids;

namespace Warehouse.Domain.SharedKernel;

/// <summary>Non-generic infrastructure hook so persistence interceptors can stamp creation audit data.</summary>
internal interface ICreationStamp
{
    void StampCreated(DateTimeOffset at, UserId? by);
}
