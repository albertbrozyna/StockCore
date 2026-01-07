using StockCore.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace StockCore.Domain.Entities
{
    public class ProductAttributeValue : ITenantEntity
    {
      
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public Guid AttributeDefinitionId { get; set; }

        public string Value { get; set; }  

        public Product Product { get; set; }
        public AttributeDefinition AttributeDefinition { get; set; }
        public Guid TenantId { get ; set; }

        public ProductAttributeValue(Guid id, Guid productId,Guid attributeDefinitionId, string value, Guid tenantId)
        {
            Id = id;
            ProductId = productId;
            AttributeDefinitionId = attributeDefinitionId;
            Value = value;
            TenantId = tenantId;
        }

    }
}
