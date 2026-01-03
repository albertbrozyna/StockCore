using CSharpFunctionalExtensions;
using StockCore.Domain.Entities;
using StockCore.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace StockCore.Application.Common.Interfaces.Authentication
{
    public interface IRegisterUserService
    {
        public Task<Result<User>> RegisterUser(User user,string password,UserRole role);
    }
}
