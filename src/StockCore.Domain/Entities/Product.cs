using StockCore.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace StockCore.Domain.Entities
{
    public class Product : ITenantEntity
    {
        public Product(int id, string name, decimal price, List<ProductAttributeValue> attributeValues, Guid tenantId)
        {
            Id = id;
            Name = name;
            Price = price;
            AttributeValues = attributeValues;
            TenantId = tenantId;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }

        public List<ProductAttributeValue> AttributeValues { get; set; } = new();
        public Guid TenantId { get ; set; }
    }
}
