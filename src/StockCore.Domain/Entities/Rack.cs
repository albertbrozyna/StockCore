using StockCore.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace StockCore.Domain.Entities
{
    public class Rack : ITenantEntity
    {
        public Guid TenantId { get; set; }
        public int Id { get; set; }
        public string RackNumber { get; set; }
        public List<Shelf> Shelves { get; set; } = new();
    }
}
