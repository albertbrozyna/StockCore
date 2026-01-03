using System;
using System.Collections.Generic;
using System.Text;

namespace StockCore.Domain.Exceptions
{
    public class UserNotFoundException : DomainException
    {
        public UserNotFoundException(Guid userId) : base($"User with Id: {userId} was not found")
        {
        }
    }
}
