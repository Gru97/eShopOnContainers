using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.API.Models
{
    public class CatalogItem
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }
        public decimal Price { get; set; }
        public int CatalogTypeId { get; set; }
        public CatalogType CatalogType { get; set; }
        public int CatalogBrandId { get; set; }
        public CatalogBrand CatalogBrand { get; set; }
        public string PictureName { get; set; }
        public string PictureUri{ get; set; }
        public int AvailableStock { get; set; }

        public CatalogItem( string name, string description, decimal price, int catalogTypeId, int catalogBrandId, string pictureName, int availableStock)
        {
            Name = name;
            Description = description;
            Price = price;
            CatalogTypeId = catalogTypeId;
            CatalogBrandId = catalogBrandId;
            PictureName = pictureName;
            AvailableStock = availableStock;
        }

        public CatalogItem()
        {
        }
    }
}
