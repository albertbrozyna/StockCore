using System;
using System.Collections.Generic;
using System.Text;

namespace StockCore.Domain.Entities
{
    public class ProductAttributeValue
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int AttributeDefinitionId { get; set; }

        public string Value { get; set; }  

        public Product Product { get; set; }
        public AttributeDefinition AttributeDefinition { get; set; }
    }
}
