using StockCore.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace StockCore.Domain.Entities
{
    public class Shelf : ITenantEntity
    {
        public Guid TenantId { get; set; }
        public int Id { get; set; }
        public int Level { get; set; }
        public string ShelfCode { get; set; }
        public double MaxCapacity { get; set; }

        public Shelf(Guid tenantId, int id, int level, string shelfCode, double maxCapacity)
        {
            TenantId = tenantId;
            Id = id;
            Level = level;
            ShelfCode = shelfCode;
            MaxCapacity = maxCapacity;
        }
    }
}
