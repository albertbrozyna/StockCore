using Warehouse.Domain.SharedKernel;
using Warehouse.Domain.SharedKernel.Abstractions;
using Warehouse.Domain.SharedKernel.Enums;
using Warehouse.Domain.SharedKernel.Ids;
using Warehouse.Domain.SharedKernel.ValueObjects;

namespace Warehouse.Domain.Catalog;

/// <summary>A supplier and/or customer.</summary>
public sealed class Contractor : SoftDeletableTenantEntity<ContractorId>, IAggregateRoot
{
    private readonly List<ContractorAddress> _addresses = [];
    private readonly List<ContractorContact> _contacts = [];
    private readonly List<ExternalReference> _externalReferences = [];

    private Contractor()
    {
        Code = null!;
        Name = null!;
    }

    public string Code { get; private set; }

    public string Name { get; private set; }

    public ContractorKind Kind { get; private set; }

    public TaxId? TaxId { get; private set; }

    public CurrencyCode? DefaultCurrency { get; private set; }

    public int? PaymentTermsDays { get; private set; }

    public bool IsActive { get; private set; }

    public IReadOnlyCollection<ContractorAddress> Addresses => _addresses;

    public IReadOnlyCollection<ContractorContact> Contacts => _contacts;

    public IReadOnlyCollection<ExternalReference> ExternalReferences => _externalReferences;

    public static Contractor Create(
        string code,
        string name,
        ContractorKind kind,
        TaxId? taxId = null,
        CurrencyCode? defaultCurrency = null,
        int? paymentTermsDays = null)
    {
        var contractor = new Contractor
        {
            Id = ContractorId.New(),
            Code = CatalogText.Required(code, 50, "Catalog.InvalidCode", nameof(code)),
            IsActive = true,
        };
        contractor.Update(name, kind, taxId, defaultCurrency, paymentTermsDays);
        return contractor;
    }

    public void Update(string name, ContractorKind kind, TaxId? taxId, CurrencyCode? defaultCurrency, int? paymentTermsDays)
    {
        if (kind == 0)
        {
            throw CatalogText.Fail("Catalog.InvalidContractorKind", "A contractor must be a supplier, a customer or both.", ErrorType.Validation);
        }

        Name = CatalogText.Required(name, 200, "Catalog.InvalidName", nameof(name));
        Kind = kind;
        TaxId = taxId;
        DefaultCurrency = defaultCurrency;
        PaymentTermsDays = paymentTermsDays is { } d ? Guard.AgainstNegative(d) : null;
    }

    /// <summary>Adds an address; a default address replaces the previous default of the same type.</summary>
    public void AddAddress(Address address, AddressType type, bool isDefault = false)
    {
        Guard.AgainstNull(address);
        if (isDefault)
        {
            _addresses.Where(a => a.Type == type).ToList().ForEach(a => a.ClearDefault());
        }

        _addresses.Add(ContractorAddress.Create(Id, address, type, isDefault));
    }

    public void AddContact(ContactInfo contact, string? role = null)
    {
        Guard.AgainstNull(contact);
        _contacts.Add(ContractorContact.Create(Id, contact, role));
    }

    public void AddExternalReference(ExternalReference reference)
    {
        Guard.AgainstNull(reference);
        if (_externalReferences.Contains(reference))
        {
            throw CatalogText.Fail("Catalog.DuplicateExternalReference", "This external reference already exists.", ErrorType.Conflict);
        }

        _externalReferences.Add(reference);
    }

    public void Activate() => IsActive = true;

    public void Deactivate() => IsActive = false;
}
