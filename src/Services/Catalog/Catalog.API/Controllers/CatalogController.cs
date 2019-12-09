using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Catalog.API.Infrastructure;
using Catalog.API.IntegrationEvents;
using Catalog.API.IntegrationEvents.Events;
using Catalog.API.Models;
using EventBus.Abstractions;
using IntegrationEventLogEF.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Catalog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private readonly Infrastructure.CatalogContext _catalogContext;
        private IEventBus _eventBus;
        private ICatalogIntegrationEventService _catalogIntegrationEventService;
        private IIntegrationEventLogService _integrationEventLogService;
        private ISearchRepository<CatalogItem> searchRepository;
        public CatalogController(CatalogContext catalogContext, IEventBus eventBus,
            ICatalogIntegrationEventService catalogIntegrationEventService,
            IIntegrationEventLogService integrationEventLogService,
            ISearchRepository<CatalogItem> searchRepository)
        {
            this._catalogContext = catalogContext;
            this._eventBus = eventBus;
            _catalogIntegrationEventService = catalogIntegrationEventService;
            _integrationEventLogService = integrationEventLogService;
            this.searchRepository = searchRepository; ;
        }



        [Route("items")]
        [HttpGet]
        public ActionResult<IEnumerable<CatalogItem>> GetAll()
        {
            //To test rabbitMQ
            //eventBus.Publish(new ProductPriceChangedIntegrationEvent(1, 10, 15) { });
            //var items = new List<CatalogItem> { }


            //To populate data to redis at the begining
            //var items = _catalogContext.CatalogItems.Include(e=>e.CatalogType).Include(e=>e.CatalogBrand).ToList();
            //searchRepository.SaveManyAsync(items);

            var items = _catalogContext.CatalogItems.ToList();
            items.ForEach(e => e.PictureUri = "https://localhost:44321/Pics/" + e.PictureName);
            return items;
        }
        [Route("items/filter")]
        [HttpGet]
        public ActionResult<CatalogItemViewModel> GetAllWithFilter([FromQuery] int pageSize,int pageIndex)
        {
            int count = 0;
            var query = _catalogContext.CatalogItems.AsQueryable();
            count = query.Count();
            query = query.OrderBy(e => e.Id)
                .Skip(pageSize * pageIndex)
                .Take(pageSize);
            query=query.Include(e => e.CatalogBrand)
                .Include(e => e.CatalogType);
            var items = query.ToList();
            items.ForEach(e => e.PictureUri = "https://localhost:44321/Pics/" + e.PictureName);

            return new CatalogItemViewModel { CatalogItem = items, Count = count };
        }

        [Route("items/{id}")]
        [HttpGet]
        public ActionResult<CatalogItem> GetById(int Id)
        {
            var item= _catalogContext.CatalogItems.SingleOrDefault(e => e.Id == Id);
            item.PictureUri = "https://localhost:44321/Pics/" + item.PictureName;
            return item;

        }

        [HttpPost]
        [Route("items")]
        public async Task CreateProductAsync(CatalogItem newProduct)
        {
            CatalogItem catalog = new CatalogItem(newProduct.Name,
                newProduct.Description, newProduct.Price, newProduct.CatalogType.Id, newProduct.CatalogBrand.Id,
                newProduct.PictureName, newProduct.AvailableStock);
            _catalogContext.Add(catalog);
            await _catalogContext.SaveChangesAsync();
        }

        [HttpPut]
        [Route("items")]
        public async Task UpdateProductAsync(CatalogItem productToUpdate)
        {
            //TODO: checking for necessity of publishing event PriceChanged
            //TODO: Other possibilities for state of event is not considered
            //To update the price of a product and guarantee consistency,
            //(that the publishing of it's related event has definitely happened after persisting in current service,
            //first we need to update CatalogContext(and product)
            //and we need to store details of the related event in database (IntegrationEventLogContext) 
            //IN THE SAME TRANSACTION
            //then we will publish the event
            //and if everything goes as expected, we change the state of event to published in db
            //TODO don't send productToUpdate to repo. fetch the current object, change it's price then send it (if fetched in admin panel before update there is no problem
            var oldPrice = _catalogContext.CatalogItems.AsNoTracking().Single(e => e.Id == productToUpdate.Id).Price;
            ProductPriceChangedIntegrationEvent evt =
                new ProductPriceChangedIntegrationEvent(productToUpdate.Id, productToUpdate.Price, oldPrice);
            using (var transaction = _catalogContext.Database.BeginTransaction())
            {
                _catalogContext.CatalogItems.Update(productToUpdate);
                await _catalogIntegrationEventService.SaveEventAndCatalogContextChangesAsync(evt);
                transaction.Commit();
            }

            _eventBus.Publish(evt);
            await _integrationEventLogService.MarkEventAsPublished(evt.Id);

        }

        [HttpGet]
        [Route("catalogTypes")]
        public async Task<IEnumerable<CatalogType>> GetTypes()
        {
            var types = await _catalogContext.CatalogTypes.ToListAsync();
            return types;

        }

        [HttpGet]
        [Route("catalogBrands")]
        public async Task<IEnumerable<CatalogBrand>> GetBrands()
        {
            var brands = await _catalogContext.CatalogBrands.ToListAsync();
            return brands;
        }
        [HttpGet]
        [Route("items/type/{id}")]
        public async Task<IEnumerable<CatalogItem>> GetProductByTypeId(int? id)
        {
            var catalogs = _catalogContext.CatalogItems.Where(e => e.CatalogType.Id == id);
            catalogs.ToList().ForEach(e => e.PictureUri = "https://localhost:44321/Pics/" + e.PictureName);

            return catalogs;

        }
        [HttpGet]
        [Route("items/type/{typeId}/brand/{brandId?}")]
        public async Task<IEnumerable<CatalogItem>> GetProductByTypeAndBrandId(int typeId,int? brandId) {
            var catalogs = _catalogContext.CatalogItems.Where(e => e.CatalogType.Id == typeId);
            if (brandId != null && brandId>0)
                catalogs=catalogs.Where(e => e.CatalogBrand.Id == brandId);

            catalogs.ToList().ForEach(e => e.PictureUri = "https://localhost:44321/Pics/" + e.PictureName);

            return catalogs;

        }

        [HttpGet]
        [Route("items/search/{phrase}")]
        public async Task<IEnumerable<CatalogItem>> Search(string phrase)
        {
            var data = _catalogContext.CatalogItems.Include(p => p.CatalogBrand).Include(p => p.CatalogType).ToList();
            data.ForEach(e => e.PictureUri = "https://localhost:44321/Pics/" + e.PictureName);
            await searchRepository.SaveManyAsync(data);

            var lst=await searchRepository.SearchAsync(phrase);
            return lst;
        }

        [HttpGet]
        [Route("items/search")]
        public async Task<IEnumerable<CatalogItem>> Search([FromQuery]SearchModel model)
        {
            var lst=await searchRepository.SearchAsync(model);
            return lst.ToList();
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existing = _catalogContext.CatalogItems.SingleOrDefault(e => e.Id == id);
            if (existing != null)
                this._catalogContext.Remove(existing);

            var result =await searchRepository.DeleteAsync(id);
            //_catalogContext.SaveChanges();
            return Ok();
        }

    }
}