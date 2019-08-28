using Catalog.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.API.Infrastructure.EntityConfigurations
{
    public class CatalogItemEntityTypeConfigurations : IEntityTypeConfiguration<CatalogItem>
    {
        public void Configure(EntityTypeBuilder<CatalogItem> builder)
        {
            builder.ToTable("Catalog");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .ForSqlServerUseSequenceHiLo()
                .IsRequired();
            builder.Property(x => x.Name)
                .HasMaxLength(100)
                .IsRequired(true);
            builder.Property(x => x.Price)
                .IsRequired(true);
            builder.Ignore(x => x.PictureUri);

            builder.HasOne(x => x.CatalogType)
                .WithMany()
                .HasForeignKey(x => x.CatalogTypeId);
            builder.HasOne(x => x.CatalogBrand)
                .WithMany()
                .HasForeignKey(x => x.CatalogBrandId);
        }
    }
}
