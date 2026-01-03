using CSharpFunctionalExtensions;
using StockCore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace StockCore.Application.Common.Interfaces.Authentication
{
    public interface IVerificationCodeService
    {
        Task<string> GenerateCode(Guid userId, CancellationToken cancellationToken);
        public Task<Result> VerifyUserEmail(Guid userId, string verificationCode, CancellationToken cancellationToken);

    }
}
