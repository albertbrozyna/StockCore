using System;
using System.Collections.Generic;
using System.Text;

namespace StockCore.Application.Common.DTOs
{
    public record ProductResponse(
            Guid Id,
            string Name,
            decimal Price,
            string Code,
            List<AttributeResponse> Attributes
        );
}
