using Basket.API.Model;
using BuildingBlocks.EventBus.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ordering.API.Application.IntegrationEvents.Events
{
    public class UserCheckoutIntegrationEvent:IntegrationEvent
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Street { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public CustomerBasket Basket { get; set; }
        public string Buyer { get; set; }

        public UserCheckoutIntegrationEvent(string userId, string userName, string street, string country, string city, string state, string zipCode, CustomerBasket basket, string buyer)
        {
            UserId = userId;
            UserName = userName;
            Street = street;
            Country = country;
            City = city;
            State = state;
            ZipCode = zipCode;
            Basket = basket;
            Buyer = buyer;
        }
    }
    
    
}
