using Warehouse.Api.Domain.SharedKernel.Ids;

namespace Warehouse.Api.Domain.SharedKernel;

/// <summary>Non-generic infrastructure hook so persistence interceptors can stamp modification audit data.</summary>
internal interface IModificationStamp
{
    void StampModified(DateTimeOffset at, UserId? by);
}
