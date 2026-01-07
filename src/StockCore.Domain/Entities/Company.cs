using StockCore.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace StockCore.Domain.Entities
{
    public class Company : ITenantEntity
    {
        public Guid TenantId { get; set; }
        public string CompanyJoinCode { get; set; }
        public string CompanyName { get; set; }
        public List<Warehouse> Warehouses { get; set; } = new();

        private Company()
        {

        }

        public Company(Guid tenantId, string companyJoinCode, string companyName)
        {
            TenantId = tenantId;
            CompanyJoinCode = companyJoinCode;
            CompanyName = companyName;
       
        }
    }
}
