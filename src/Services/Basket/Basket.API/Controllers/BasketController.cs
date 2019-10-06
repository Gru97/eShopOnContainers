using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Basket.API.Model;
using EventBus.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

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
            //repository.UpdateBasketAsync(new CustomerBasket("1")
            //{
            //    CustomerId = "1",
            //    Items = new List<BasketItem>() { new BasketItem() { Id = 1,UnitPrice = 50000,Quantity = 2,ProductName = "Lenovo E560", PictureUri = "https://localhost:44321/Pics/phone/m1.jpg" } }
            //});
        }

        
        
        
        [HttpGet]
        [Route("Get/{Id}")]
        public async Task<ActionResult< CustomerBasket>>  GetBasketByIdAsync(string Id)
        {
            var basket=await repository.GetBasketAsync(Id);
            if (basket == null)
            {
                basket=new CustomerBasket(Id);

            }
            return basket;
        }

        [HttpDelete]
        [Route("Delete/Id")]
        public async Task<bool> DeleteBasketByIdAsync(string CustomerId)
        {
            return await repository.DeleteBasketAsync(CustomerId);
        }

        [HttpPost]
        [Route("Update")]
        public async Task<ActionResult<CustomerBasket>> UpdateBasketAsync([FromBody] CustomerBasket basket)
        {
            //CustomerBasket c = new CustomerBasket("1");
            //c.Items.Add(new BasketItem { Id = 1, ProductName = "p1" });

            return await repository.UpdateBasketAsync(basket);

        }

    }
}
