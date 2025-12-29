using StockCore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace StockCore.Application.Common.Interfaces
{
    public interface IUserDetails
    {
        User User { get; set; }

    }
}
