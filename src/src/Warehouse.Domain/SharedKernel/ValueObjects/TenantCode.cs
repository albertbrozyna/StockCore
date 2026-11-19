using System.Text.RegularExpressions;

namespace Warehouse.Domain.SharedKernel.ValueObjects;

/// <summary>Tenant slug matching <c>[a-z0-9-]{3,40}</c>.</summary>
public sealed partial record TenantCode
{
    private TenantCode(string value) => Value = value;

    public string Value { get; }

    public static TenantCode Create(string value)
    {
        var text = Guard.AgainstNullOrWhiteSpace(value).Trim();
        return Slug().IsMatch(text)
            ? new TenantCode(text)
            : throw ValueObjectErrors.Invalid("Common.InvalidTenantCode", "Tenant code must match [a-z0-9-]{3,40}.");
    }

    public override string ToString() => Value;

    [GeneratedRegex("^[a-z0-9-]{3,40}$", RegexOptions.CultureInvariant)]
    private static partial Regex Slug();
}
