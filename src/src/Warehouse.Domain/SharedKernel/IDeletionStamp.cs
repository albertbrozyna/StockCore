using Warehouse.Domain.SharedKernel.Ids;

namespace Warehouse.Domain.SharedKernel;

/// <summary>Non-generic infrastructure hook so persistence interceptors can soft-delete and stamp deletion data.</summary>
internal interface IDeletionStamp
{
    void Delete();

    void StampDeleted(DateTimeOffset at, UserId? by);
}
