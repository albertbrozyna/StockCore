using System;
using System.Collections.Generic;
using System.Text;

namespace StockCore.Domain.Exceptions
{
    public class UserNotFoundException : DomainException
    {
        public UserNotFoundException(string message) : base(message)
        {
        }
    }
}
