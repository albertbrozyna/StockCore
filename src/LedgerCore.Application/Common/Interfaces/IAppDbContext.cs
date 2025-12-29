using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace StockCore.Application.Common.Interfaces
{
    public interface IAppDbContext
    {
        DbSet<User> Users { get; }
        DbSet<RefreshToken>RefreshTokens { get; }
        DbSet<UserVerificationToken> UserVerificationTokens { get;  }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
