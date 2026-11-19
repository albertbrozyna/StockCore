namespace Warehouse.Api.Domain.SharedKernel;

/// <summary>Describes a failure. <paramref name="Code"/> has the form <c>Context.Name</c>, e.g. <c>Inventory.InsufficientStock</c>.</summary>
public sealed record Error(string Code, string Message, ErrorType Type)
{
    /// <summary>Placeholder carried by successful results.</summary>
    public static readonly Error None = new(string.Empty, string.Empty, ErrorType.Unexpected);
}
