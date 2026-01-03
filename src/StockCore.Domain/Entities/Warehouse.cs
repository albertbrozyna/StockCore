using StockCore.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace StockCore.Domain.Entities
{
    public class Warehouse : ITenantEntity
    {
        public Guid TenantId { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public List<WarehouseHall> Halls { get; set; } = new();
    }
}
