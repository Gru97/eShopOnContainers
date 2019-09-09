using Catalog.API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.API.Infrastructure.EntityConfigurations
{
    public class CatalogBrandEntityTypeConfiguration : IEntityTypeConfiguration<CatalogBrand>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<CatalogBrand> builder)
        {
            builder.ToTable("CatalogBrand");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .ForSqlServerUseSequenceHiLo()
                .IsRequired();
            builder.Property(x => x.Brand)
                .HasMaxLength(100)
                .IsRequired();

            builder.HasData(new List<CatalogBrand>()
            {
                new CatalogBrand(){Id = 1,Brand = "Lenovo"},
                new CatalogBrand(){Id=2,Brand = "LG"}
            });
        }
    }
}
