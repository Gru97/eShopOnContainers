using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Basket.API.Model
{
    public class CustomerBasket
    {
        public List<BasketItem> Items { get; set; }
        public string BuyerId  { get; set; }

        public CustomerBasket(string Id)
        {
            BuyerId = Id;
            Items=new List<BasketItem>();
        }
    }
}
