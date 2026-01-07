using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StockCore.Domain.Entities;

namespace StockCore.Infrastructure.Persistence.Configuration
{
    internal class AttributeDefinitionConfiguration : IEntityTypeConfiguration<AttributeDefinition>
    {
        public void Configure(EntityTypeBuilder<AttributeDefinition> builder)
        {
            builder.ToTable("AttributeDefinitions");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
               .IsRequired()
               .HasMaxLength(100);

            builder.Property(x => x.DataType)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.HasIndex(x => new { x.TenantId, x.Name })
             .IsUnique();
        }
    }
}
