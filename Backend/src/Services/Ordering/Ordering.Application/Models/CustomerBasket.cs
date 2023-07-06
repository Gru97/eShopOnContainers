using System.Collections.Generic;

namespace Ordering.Application.Models
{
    //Exactly the model used in basket. Needed because of the event that is sent from basket service
    public class CustomerBasket
    {
        public List<BasketItem> Items { get; set; }
        public string CustomerId { get; set; }

        public CustomerBasket(string Id)
        {
            CustomerId = Id;
            Items = new List<BasketItem>();
        }
    }
}