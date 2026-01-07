using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StockCore.Domain.Entities;

namespace StockCore.Infrastructure.Persistence.Configuration
{
    internal class ProductAttributeValueConfiguration : IEntityTypeConfiguration<ProductAttributeValue>
    {
        public void Configure(EntityTypeBuilder<ProductAttributeValue> builder)
        {
            builder.ToTable("ProductAttributeValues");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Value)
                   .IsRequired();

            builder.Property(x => x.TenantId)
                   .IsRequired();


            builder.HasOne(pav => pav.Product).WithMany(p => p.AttributeValues)
               .HasForeignKey(p => p.ProductId)
               .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(pav => pav.AttributeDefinition)
                .WithMany()
                .HasForeignKey(pav => pav.AttributeDefinitionId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => new { x.ProductId, x.AttributeDefinitionId}).IsUnique();
        }
    }
}
