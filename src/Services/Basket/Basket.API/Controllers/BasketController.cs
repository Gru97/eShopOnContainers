﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Basket.API.Model;
using EventBus.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Ordering.API.Application.IntegrationEvents.Events;

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
            CustomerBasket c = new CustomerBasket(basket.CustomerId);
            c.Items.Add(new BasketItem { Id = 1, ProductName = "p1" });

            return await repository.UpdateBasketAsync(basket);

        }

        [Route("checkout")]
        //[Authorize]
        [HttpPost]
        public async Task<ActionResult> CheckoutAsync([FromBody] BasketCheckout basketCheckout)
        {
            //Publish UserCheckoutIntegrationEvent and dispatch it throught eventbus

            //var userIdentity = User.FindFirstValue(ClaimTypes.NameIdentifier); // will give the user's userId
            CustomerBasket basket = await repository.GetBasketAsync(basketCheckout.UserId);
            if (basket == null) return BadRequest();
            var message = new UserCheckoutIntegrationEvent(basketCheckout.UserIdentity,
                basketCheckout.UserName,
                basketCheckout.Street, basketCheckout.Country,
                basketCheckout.City, basketCheckout.State, basketCheckout.ZipCode,
                basket);
            message.Id = new Guid();
            eventBus.Publish(message);
            return Accepted();
        }

    }
}
