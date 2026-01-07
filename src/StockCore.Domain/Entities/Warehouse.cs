using StockCore.Domain.Interfaces;

namespace StockCore.Domain.Entities
{
    public class Warehouse : ITenantEntity
    {
        public Guid Id { get; set; }
        public Guid TenantId { get; set; }

        public string Name { get; set; }
        public List<WarehouseHall> Halls { get; set; } = new();

        private Warehouse() { }

        public Warehouse(Guid id, Guid tenantId, string name)
        {
            Id = id;
            TenantId = tenantId;
            Name = name;
        }
    }
}
