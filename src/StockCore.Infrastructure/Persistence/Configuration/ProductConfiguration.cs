using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StockCore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace StockCore.Infrastructure.Persistence.Configuration
{
    internal class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
              .IsRequired()
              .HasMaxLength(200);

            builder.Property(x => x.Code)
                   .HasMaxLength(50);

            builder.Property(x => x.Price)
                   .HasPrecision(18, 2);

            builder.Property(x => x.TenantId)
                   .IsRequired();

            builder.HasIndex(x => new { x.TenantId, x.Code })
                   .IsUnique();
        }
    }
}
