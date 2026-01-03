using System;
using System.Collections.Generic;
using System.Text;

namespace StockCore.Domain.Interfaces
{
    public interface ITenantEntity
    {
        Guid TenantId { get; set; }
    }
}
