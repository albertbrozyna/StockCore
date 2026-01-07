using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StockCore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace StockCore.Infrastructure.Persistence.Configuration
{
    public class CompanyConfiguration : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            builder.ToTable("Companies");

            builder.HasKey(x => x.TenantId);


        }
    }
}
