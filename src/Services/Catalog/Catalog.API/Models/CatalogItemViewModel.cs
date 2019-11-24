using System.Collections.Generic;
using Catalog.API.Models;

namespace Catalog.API.Models
{
    public class CatalogItemViewModel
    {
        public List<CatalogItem> CatalogItem { get; set; }
        public int Count { get; set; }
    }
}