using System.Net.Mail;

namespace Warehouse.Domain.SharedKernel.ValueObjects;

/// <summary>Optional e-mail, phone and contact person. A non-empty e-mail must be well-formed.</summary>
public sealed record ContactInfo
{
    private ContactInfo(string? email, string? phone, string? personName)
    {
        Email = email;
        Phone = phone;
        PersonName = personName;
    }

    public string? Email { get; }

    public string? Phone { get; }

    public string? PersonName { get; }

    public static ContactInfo Create(string? email, string? phone, string? personName)
    {
        var mail = Normalize(email);
        if (mail is not null && !IsValidEmail(mail))
        {
            throw ValueObjectErrors.Invalid("Common.InvalidEmail", $"'{mail}' is not a valid e-mail address.");
        }

        return new ContactInfo(mail, Normalize(phone), Normalize(personName));
    }

    private static bool IsValidEmail(string value) =>
        MailAddress.TryCreate(value, out var parsed) && parsed.Address == value && parsed.Host.Contains('.', StringComparison.Ordinal);

    private static string? Normalize(string? value) => string.IsNullOrWhiteSpace(value) ? null : value.Trim();
}
