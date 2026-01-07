using StockCore.Domain.Interfaces;

namespace StockCore.Domain.Entities
{
    public class Shelf : ITenantEntity
    {
        public Guid Id { get; set; }
        public Guid TenantId { get; set; }
        public int Level { get; set; }
        public string ShelfCode { get; set; }
        public double MaxCapacity { get; set; }
        public bool IsFull { get; private set; }

        public Guid RackId { get; set; }
        private Shelf() { }
        public Shelf(Guid id, Guid tenantId, int level, string shelfCode, double maxCapacity)
        {
            TenantId = tenantId;
            Id = id;
            Level = level;
            ShelfCode = shelfCode;
            MaxCapacity = maxCapacity;
        }
    }
}
