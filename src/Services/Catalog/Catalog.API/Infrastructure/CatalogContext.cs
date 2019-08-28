using Catalog.API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.API.Infrastructure
{
    public class CatalogContext : DbContext
    {
        public CatalogContext(DbContextOptions<CatalogContext> options) : base(options)
        { }

        public DbSet<CatalogType> CatalogTypes { get; set; }
        public DbSet<CatalogItem> CatalogItems { get; set; }
        public DbSet<CatalogBrand>  CatalogBrands { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new EntityConfigurations.CatalogBrandEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new EntityConfigurations.CatalogItemEntityTypeConfigurations());
            modelBuilder.ApplyConfiguration(new EntityConfigurations.CatalogTypeEntityTypeConfigurations());

            //base.OnModelCreating(modelBuilder);
        }
    }
}
