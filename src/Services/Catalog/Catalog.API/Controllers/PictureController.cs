using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Catalog.API.Infrastructure;
using Catalog.API.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Catalog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PictureController : ControllerBase
    {
        private readonly CatalogContext _catalogContext;
        private ISearchRepository<Models.CatalogItem> searchRepository;

        public PictureController(CatalogContext catalogContext, ISearchRepository<CatalogItem> searchRepository)
        {
            _catalogContext = catalogContext;
            this.searchRepository = searchRepository;
        }

        [HttpPost]
        [Route("upload/{productId}")]
        public async Task<ActionResult<bool>> Upload(int productId)
        {

            try
            {
                var existingProduct = _catalogContext.CatalogItems
                    .Include(e => e.CatalogType)
                    .Single(e => e.Id == productId);

                var subFolder = existingProduct.CatalogType.Type;
                var file = Request.Form.Files[0];
                string folderName = $"Pics/{subFolder}";
                if (!Directory.Exists(folderName))
                {
                    Directory.CreateDirectory(folderName);
                }
                if (file.Length > 0)
                {
                    string fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    string fullPath = Path.Combine(folderName, fileName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    existingProduct.PictureName = subFolder+"/"+fileName;
                    _catalogContext.SaveChanges();
                    await searchRepository.UpdateAsync(productId,new { PicureName= subFolder + fileName });
                }
                return true;
            }
            catch (System.Exception ex)
            {
                return false;
            }

        }
    }
}