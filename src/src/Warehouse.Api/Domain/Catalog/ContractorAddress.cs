using Warehouse.Api.Domain.SharedKernel;
using Warehouse.Api.Domain.SharedKernel.Enums;
using Warehouse.Api.Domain.SharedKernel.Ids;
using Warehouse.Api.Domain.SharedKernel.ValueObjects;

namespace Warehouse.Api.Domain.Catalog;

/// <summary>Address of a contractor. Managed through <see cref="Contractor"/>.</summary>
public sealed class ContractorAddress : TenantEntity<ContractorAddressId>
{
    private ContractorAddress()
    {
        Address = null!;
    }

    public ContractorId ContractorId { get; private set; }

    public Address Address { get; private set; }

    public AddressType Type { get; private set; }

    public bool IsDefault { get; private set; }

    internal static ContractorAddress Create(ContractorId contractorId, Address address, AddressType type, bool isDefault) =>
        new()
        {
            Id = ContractorAddressId.New(),
            ContractorId = contractorId,
            Address = address,
            Type = type,
            IsDefault = isDefault,
        };

    internal void ClearDefault() => IsDefault = false;
}
