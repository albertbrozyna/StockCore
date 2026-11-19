using Warehouse.Api.Domain.SharedKernel;
using Warehouse.Api.Domain.SharedKernel.Ids;
using Warehouse.Api.Domain.SharedKernel.ValueObjects;

namespace Warehouse.Api.Domain.Catalog;

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
