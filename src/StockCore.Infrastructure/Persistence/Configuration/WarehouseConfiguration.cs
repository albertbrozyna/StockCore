using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StockCore.Domain.Entities;

namespace StockCore.Infrastructure.Persistence.Configuration
{
    internal class WarehouseConfiguration : IEntityTypeConfiguration<Warehouse>
    {
        public void Configure(EntityTypeBuilder<Warehouse> builder)
        {
            builder.ToTable("Warehouses");

            builder.Property(x => x.Name).IsRequired().HasMaxLength(100).HasComment("Unique name of magazine").HasColumnName("Name");

            builder.HasIndex(x => x.Name).IsUnique();
        }
    }
}
