using Warehouse.Domain.SharedKernel;
using Warehouse.Domain.SharedKernel.Abstractions;
using Warehouse.Domain.SharedKernel.Ids;

namespace Warehouse.Domain.Catalog;

/// <summary>Node of the product category tree.</summary>
public sealed class SkuCategory : SoftDeletableTenantEntity<SkuCategoryId>, IAggregateRoot
{
    private SkuCategory()
    {
        Code = null!;
        Name = null!;
        Path = null!;
    }

    public string Code { get; private set; }

    public string Name { get; private set; }

    public SkuCategoryId? ParentId { get; private set; }

    /// <summary>Materialized path of ids from the root, e.g. <c>/{rootId}/{id}/</c> (ids as 32-digit hex).</summary>
    public string Path { get; private set; }

    public static SkuCategory Create(string code, string name, SkuCategory? parent = null)
    {
        var id = SkuCategoryId.New();
        return new SkuCategory
        {
            Id = id,
            Code = CatalogText.Required(code, 50, "Catalog.InvalidCode", nameof(code)),
            Name = CatalogText.Required(name, 200, "Catalog.InvalidName", nameof(name)),
            ParentId = parent?.Id,
            Path = $"{parent?.Path ?? "/"}{id.Value:N}/",
        };
    }
}
