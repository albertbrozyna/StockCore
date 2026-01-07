using System;
using System.Collections.Generic;
using System.Text;

namespace StockCore.Domain.Exceptions
{
    public class IncorrectNumberOfLevelsException : DomainException
    {
        public IncorrectNumberOfLevelsException(string message) : base(message)
        {
        }
    }
}
