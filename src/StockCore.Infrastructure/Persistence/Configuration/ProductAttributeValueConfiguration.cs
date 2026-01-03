using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StockCore.Domain.Entities;

namespace StockCore.Infrastructure.Persistence.Configuration
{
    internal class ProductAttributeValueConfiguration : IEntityTypeConfiguration<ProductAttributeValue>
    {
        public void Configure(EntityTypeBuilder<ProductAttributeValue> builder)
        {
            builder.HasOne(pav => pav.Product).WithMany(p => p.AttributeValues).HasForeignKey(p => p.ProductId);
            builder.HasOne(pav => pav.AttributeDefinition).WithMany().HasForeignKey(pav => pav.AttributeDefinitionId);
        }
    }
}
