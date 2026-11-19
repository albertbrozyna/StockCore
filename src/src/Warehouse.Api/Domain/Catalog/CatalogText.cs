using Warehouse.Api.Domain.SharedKernel;

namespace Warehouse.Api.Domain.Catalog;

internal static class CatalogText
{
    internal static string Required(string? value, int maxLength, string errorCode, string name)
    {
        var trimmed = Guard.AgainstNullOrWhiteSpace(value, name).Trim();
        return trimmed.Length > maxLength
            ? throw Fail(errorCode, $"{name} must have at most {maxLength} characters.", ErrorType.Validation)
            : trimmed;
    }

    internal static DomainException Fail(string code, string message, ErrorType type) =>
        new(new Error(code, message, type));
}
