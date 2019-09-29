using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Basket.API.Model
{
    public class CustomerBasket
    {
        public List<BasketItem> Items { get; set; }
        public int CustomerId  { get; set; }

        public CustomerBasket(int Id)
        {
            CustomerId = Id;
            Items=new List<BasketItem>();
        }
    }
}
