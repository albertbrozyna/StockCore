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
        DbSet<Product> Products { get; }
        DbSet<ProductAttributeValue> ProductAttributeValues { get; }
        DbSet<AttributeDefinition> AttributeDefinitions { get; }
        DbSet<Warehouse> Warehouses { get; }
        DbSet<WarehouseHall>WarehouseHalls { get; }
        DbSet<Rack>Racks { get; }
        DbSet<Shelf> Shelfs { get;  }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
