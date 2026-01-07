using StockCore.Domain.Interfaces;

namespace StockCore.Domain.Entities
{
    public class Product : ITenantEntity
    {
        public Guid Id { get; set; }
        public Guid TenantId { get; set; }

        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public Guid ShelfId { get; set; } 
        public Shelf Shelf { get; set; }
        public List<ProductAttributeValue> AttributeValues { get; set; } = new();

        public Product(Guid id, Guid tenantId, string name, decimal price, string description,string code)
        {
            Id = id;
            Name = name;
            Price = price;
            TenantId = tenantId;
            Description = description;
            Code = code;
        }
    }
}
