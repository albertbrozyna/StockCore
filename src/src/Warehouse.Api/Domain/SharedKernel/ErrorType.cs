namespace Warehouse.Api.Domain.SharedKernel;

/// <summary>Category of an <see cref="Error"/>.</summary>
public enum ErrorType
{
    Validation,
    NotFound,
    Conflict,
    Forbidden,
    Unexpected,
}
