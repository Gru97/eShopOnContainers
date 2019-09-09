using Catalog.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.API.Infrastructure.EntityConfigurations
{
    public class CatalogTypeEntityTypeConfigurations : IEntityTypeConfiguration<CatalogType>
    {
        public void Configure(EntityTypeBuilder<CatalogType> builder)
        {
            builder.ToTable("CatalogType");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .ForSqlServerUseSequenceHiLo("seq_name")
                .IsRequired();
            builder.Property(x => x.Type)
                .IsRequired()
                .HasMaxLength(100);
            builder.HasData(new List<CatalogType>()
            {
                new CatalogType() {Id = 1,Type = "Laptop"},
                new CatalogType() { Id = 2,Type = "Phone"}
            });
        }
    }
}
