using System;
using System.Collections.Generic;
using System.Text;

namespace StockCore.Domain.Exceptions
{
    public class IncorrectNumberOfSectionsException : DomainException
    {
        public IncorrectNumberOfSectionsException(string message) : base(message)
        {
        }
    }
}
