using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Basket.API.Model;
using EventBus.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Basket.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository repository;
        private readonly IEventBus eventBus;

        public BasketController(IBasketRepository repository, IEventBus eventBus)
        {
            this.repository = repository;
            this.eventBus = eventBus;
        }

        
        
        // GET api/values
        [HttpGet]
        public async Task<ActionResult< CustomerBasket>>  Get()
        {
            CustomerBasket c = new CustomerBasket("1");
            c.Items.Add(new BasketItem { Id = 1, ProductName = "p1" });

            return await repository.UpdateBasketAsync(c);
        }

    }
}
