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
        

    }
}
