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

            builder.HasData(new List<CatalogItem>()
            {
                new CatalogItem() {Id = 1,Price = 5000000,AvailableStock = 100,CatalogBrandId = 1,CatalogTypeId = 1,Name = "Lenovo Thinkpad E560"},
                new CatalogItem() {Id = 2,Price = 1500000,AvailableStock = 115,CatalogBrandId = 2,CatalogTypeId = 2,Name = "LG K9" }
            });
        }
    }
}
