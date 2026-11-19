using Warehouse.Api.Domain.SharedKernel.Ids;

namespace Warehouse.Api.Application.Abstractions;

/// <summary>The authenticated caller. Implemented by the API layer.</summary>
public interface ICurrentUser
{
    UserId? UserId { get; }

    string? Name { get; }

    IReadOnlySet<string> Roles { get; }

    bool HasPermission(string permission);
}
