using Basket.API.Model;

namespace Basket.API.Controllers
{
    public class BasketCheckout
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Street { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }

        
    }
}