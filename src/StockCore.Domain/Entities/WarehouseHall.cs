using StockCore.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace StockCore.Domain.Entities
{
    public class WarehouseHall : ITenantEntity
    {
        public Guid TenantId { get; set; }
        public int Id { get; set; }
        public string HallCode { get; set; }
        public List<Rack> Racks { get; set; } = new();
    }
}
