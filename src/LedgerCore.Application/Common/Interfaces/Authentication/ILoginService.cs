using CSharpFunctionalExtensions;
using StockCore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace StockCore.Application.Common.Interfaces.Authentication
{
    public interface ILoginUserService
    {
        public Task<Result<User>> LoginUser(string email, string password);
        public Task<IList<string>> GetUserRoles(User user);

    }
}