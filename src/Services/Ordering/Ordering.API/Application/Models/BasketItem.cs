using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ordering.API.Application.Models
{
    //Exactly the model used in basket. Needed because of the event that is sent from basket service

    public class BasketItem
    {
        public int Id { get; set; }
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal OldPrice { get; set; }
        public int Quantity { get; set; }
        public string PictureUri { get; set; }

    }

   
}
