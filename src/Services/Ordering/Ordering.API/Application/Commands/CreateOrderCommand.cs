using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ordering.API.Application.Commands
{
    //Commands are requests that must be proccessed once and by one receiver
    //They must be immutable since we don't need to change them after being created
    //Since we are going to need to seriallize and deserialize them, we should make them "private set" and use attributes
    //
    public class CreateOrderCommand
    {
        private readonly List<OrderItemDto> orderItems;
        public IEnumerable<OrderItemDto> OrderItems { get { return orderItems; } }
        public string UserId { get; private set; }
        public string UserName { get; private set; }
        public string City { get; private set; }
        public string State { get; private set; }
        public string Country { get; private set; }
        public string Street { get; private set; }
        public string ZipCode { get; private set; }
        public CreateOrderCommand()
        {
            orderItems = new List<OrderItemDto>();
        }
        public CreateOrderCommand(List<OrderItemDto> basketItems, string userId,
            string userName, string city, string state, string country,
            string street, string zipCode)
        {
            orderItems = basketItems.ToList();
            UserId = userId;
            UserName = userName;
            City = city;
            State = state;
            Country = country;
            Street = street;
            ZipCode = zipCode;
        }
    }
    

    public class OrderItemDto
    {
        public int ProductId { get; private set; }
        public string ProductName { get; private set; }
        public int Quantity { get; private set; }
        public decimal UnitPrice { get; private set; }
        public decimal Discount { get; set; }
    }
}
