using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using StockCore.Application.Common.Interfaces;
using StockCore.Domain.Entities;
using StockCore.Domain.Interfaces;
using System.Linq.Expressions;
namespace StockCore.Infrastructure.Persistence.Context;

public class StockDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>, IAppDbContext
{
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<UserVerificationToken> UserVerificationTokens { get; set; }

    public DbSet<Company> Companies { get; set; }

    public DbSet<Product> Products { get; set; }

    public DbSet<ProductAttributeValue> ProductAttributeValues { get; set; }

    public DbSet<AttributeDefinition> AttributeDefinitions { get; set; }

    private readonly IUserDetails _userDetails;
    public StockDbContext(DbContextOptions<StockDbContext> options,IUserDetails userDetails) : base(options)
    {
        _userDetails = userDetails;
    }

    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            var tableName = entity.GetTableName();
            if (tableName != null)
            {
                entity.SetTableName(tableName.ToLowerInvariant());
            }

            // Query filter for tenantId
            if (typeof(ITenantEntity).IsAssignableFrom(entity.ClrType))
            {
                modelBuilder.Entity(entity.ClrType)
                    .HasQueryFilter(ConvertFilterExpression(entity.ClrType));
                modelBuilder.Entity(entity.ClrType)
                    .Property(nameof(ITenantEntity.TenantId))
                    .IsRequired();
            }

        }

   

    modelBuilder.ApplyConfigurationsFromAssembly(typeof(StockDbContext).Assembly);
    }

    // Helper to build lambda
    private LambdaExpression ConvertFilterExpression(Type entityType)
    {
        var parameter = Expression.Parameter(entityType, "e");
        var property = Expression.Property(parameter, nameof(ITenantEntity.TenantId));

        var userDetailsExpression = Expression.Constant(_userDetails);
        var tenantIdProperty = Expression.Property(userDetailsExpression, nameof(IUserDetails.TenantId));

        var body = Expression.Equal(property, tenantIdProperty);
        return Expression.Lambda(body, parameter);
    }
}