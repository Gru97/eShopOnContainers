using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catalog.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private Infrastructure.CatalogContext context;
        public CatalogController(Infrastructure.CatalogContext context)
        {
            this.context = context;
        }
        [Route("items")]
        [HttpGet]
        public ActionResult<IEnumerable<CatalogItem>> GetAll()
        {
            var items=context.CatalogItems.ToList();
            return items;
        }
    }
}