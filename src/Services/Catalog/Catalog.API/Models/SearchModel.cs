using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.API.Models
{
    public class SearchModel
    {
        public List<int?> TypeId { get; set; }
        public List<int?> BrandId { get; set; }

        public string Name { get; set; }
        public long? PriceFrom { get; set; }
        public long? PriceTo { get; set; }


    }
}
