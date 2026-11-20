namespace Warehouse.Domain.SharedKernel.Enums;

public sealed class DocumentType : Enumeration
{
    public static readonly DocumentType Receipt = new(1, nameof(Receipt), "PZ");
    public static readonly DocumentType Issue = new(2, nameof(Issue), "WZ");
    public static readonly DocumentType Transfer = new(3, nameof(Transfer), "MM");
    public static readonly DocumentType InternalReceipt = new(4, nameof(InternalReceipt), "PW");
    public static readonly DocumentType InternalIssue = new(5, nameof(InternalIssue), "RW");
    public static readonly DocumentType Correction = new(6, nameof(Correction), "KOR");

    private DocumentType(int id, string name, string code, bool requiresApproval = true, bool affectsStock = true)
        : base(id, name)
    {
        Code = code;
        RequiresApproval = requiresApproval;
        AffectsStock = affectsStock;
    }

    /// <summary>Document abbreviation used in numbering (PZ, WZ, MM, PW, RW, KOR).</summary>
    public string Code { get; }

    public bool RequiresApproval { get; }

    public bool AffectsStock { get; }
}
