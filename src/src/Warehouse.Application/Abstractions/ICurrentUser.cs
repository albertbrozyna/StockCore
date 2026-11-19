using Warehouse.Domain.SharedKernel.Ids;

namespace Warehouse.Application.Abstractions;

/// <summary>The authenticated caller. Implemented by the API layer.</summary>
public interface ICurrentUser
{
    UserId? UserId { get; }

    string? Name { get; }

    IReadOnlySet<string> Roles { get; }

    bool HasPermission(string permission);
}
