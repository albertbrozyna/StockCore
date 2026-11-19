using Warehouse.Domain.SharedKernel;
using Warehouse.Domain.SharedKernel.Ids;
using Warehouse.Domain.SharedKernel.ValueObjects;

namespace Warehouse.Domain.Catalog;

/// <summary>Contact person of a contractor. Managed through <see cref="Contractor"/>.</summary>
public sealed class ContractorContact : TenantEntity<ContractorContactId>
{
    private ContractorContact()
    {
        Contact = null!;
    }

    public ContractorId ContractorId { get; private set; }

    public ContactInfo Contact { get; private set; }

    public string? Role { get; private set; }

    internal static ContractorContact Create(ContractorId contractorId, ContactInfo contact, string? role) =>
        new()
        {
            Id = ContractorContactId.New(),
            ContractorId = contractorId,
            Contact = contact,
            Role = string.IsNullOrWhiteSpace(role) ? null : role.Trim(),
        };
}
