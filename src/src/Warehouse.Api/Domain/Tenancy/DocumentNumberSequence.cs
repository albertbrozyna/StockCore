using Warehouse.Api.Domain.SharedKernel;
using Warehouse.Api.Domain.SharedKernel.Enums;
using Warehouse.Api.Domain.SharedKernel.Ids;

namespace Warehouse.Api.Domain.Tenancy;

/// <summary>Counter issuing consecutive document numbers for one (type, warehouse, year) scope.</summary>
public sealed class DocumentNumberSequence : TenantEntity<DocumentNumberSequenceId>
{
    private DocumentNumberSequence()
    {
        DocumentType = null!;
        Prefix = null!;
    }

    public DocumentType DocumentType { get; private set; }

    /// <summary><c>null</c> = tenant-wide numbering.</summary>
    public WarehouseId? WarehouseId { get; private set; }

    /// <summary><c>null</c> = no yearly reset.</summary>
    public int? Year { get; private set; }

    public string Prefix { get; private set; }

    /// <summary>Last issued number (0 = none yet).</summary>
    public long LastValue { get; private set; }

    public static DocumentNumberSequence Create(DocumentType documentType, string prefix, WarehouseId? warehouseId = null, int? year = null)
    {
        if (year is { } y)
        {
            Guard.AgainstOutOfRange(y, 1900, 9999);
        }

        return new DocumentNumberSequence
        {
            Id = DocumentNumberSequenceId.New(),
            DocumentType = Guard.AgainstNull(documentType),
            Prefix = Guard.AgainstNullOrWhiteSpace(prefix).Trim(),
            WarehouseId = warehouseId,
            Year = year,
        };
    }

    /// <summary>Issues the next number. The caller must hold a row lock (or retry on a concurrency conflict).</summary>
    public long Next() => checked(++LastValue);
}
