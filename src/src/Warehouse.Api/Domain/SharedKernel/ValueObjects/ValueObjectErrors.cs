namespace Warehouse.Api.Domain.SharedKernel.ValueObjects;

internal static class ValueObjectErrors
{
    internal static DomainException Invalid(string code, string message) =>
        new(new Error(code, message, ErrorType.Validation));

    /// <summary>Trims a required string and checks its length against <paramref name="maxLength"/>.</summary>
    internal static string BoundedText(string? value, int maxLength, string errorCode, string name)
    {
        var trimmed = Guard.AgainstNullOrWhiteSpace(value, name).Trim();
        return trimmed.Length > maxLength
            ? throw Invalid(errorCode, $"{name} must have at most {maxLength} characters.")
            : trimmed;
    }
}
