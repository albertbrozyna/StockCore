using StockCore.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace StockCore.Domain.Entities
{
    public class Company : ITenantEntity
    {
        public Company(Guid tenantId, string companyJoinCode, string companyName, List<Warehouse> warehouses)
        {
            TenantId = tenantId;
            CompanyJoinCode = companyJoinCode;
            CompanyName = companyName;
            Warehouses = warehouses;
        }

        public Guid TenantId { get; set; }
        public string CompanyJoinCode { get; set; }
        public string CompanyName { get; set; }
        public List<Warehouse> Warehouses { get; set; } = new();
    }
}
