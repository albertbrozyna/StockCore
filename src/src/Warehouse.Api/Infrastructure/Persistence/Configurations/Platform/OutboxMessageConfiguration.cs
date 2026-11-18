using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Warehouse.Api.Infrastructure.Persistence.Outbox;

namespace Warehouse.Api.Infrastructure.Persistence.Configurations.Platform;

internal sealed class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.ToTable("outbox_messages");
        builder.HasKey(m => m.Id);
        builder.Property(m => m.Id).ValueGeneratedNever();
        builder.Property(m => m.Type).HasMaxLength(300).IsRequired();
        builder.Property(m => m.Payload).HasColumnType("jsonb").IsRequired();
        builder.Property(m => m.LastError).HasMaxLength(2000);
        builder.HasIndex(m => m.OccurredAt).HasFilter("\"ProcessedAt\" IS NULL");
    }
}
