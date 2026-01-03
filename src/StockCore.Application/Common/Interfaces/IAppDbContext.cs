using Microsoft.EntityFrameworkCore;
using StockCore.Domain.Entities;

namespace StockCore.Application.Common.Interfaces
{
    public interface IAppDbContext
    {
        DbSet<User> Users { get; }
        DbSet<RefreshToken> RefreshTokens { get; }
        DbSet<UserVerificationToken> UserVerificationTokens { get; }
        DbSet<Company> Companies { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
