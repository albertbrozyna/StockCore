using StockCore.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace StockCore.Domain.Entities
{
    public class WarehouseHall : ITenantEntity
    {
        public Guid Id { get; set; }
        public Guid TenantId { get; set; }
        public string HallCode { get; set; }
        public List<Rack> Racks { get; set; } = new();

        public Guid WarehouseId { get; private set; }

        private WarehouseHall() { }
        public WarehouseHall(Guid id,Guid tenantId,Guid warehouseId , string hallCode)
        {
            TenantId = tenantId;
            Id = id;
            HallCode = hallCode;
            WarehouseId = warehouseId;
        }
    }
}
