using StockCore.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace StockCore.Domain.Entities
{
    public class AttributeDefinition : ITenantEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; } // ex color
        public string DataType { get; set; } // ex. string, int
        public Guid TenantId { get ; set ; }

        public AttributeDefinition(Guid id, string name, string dataType, Guid tenantId)
        {
            Id = id;
            Name = name;
            DataType = dataType;
            TenantId = tenantId;
        }
    }
}
