using MediatR;
using Ordering.API.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Ordering.API.Application.Commands
{
    //Commands are requests that must be proccessed once and by one receiver
    //They must be immutable since we don't need to change them after being created
    //Since we are going to need to seriallize and deserialize them, we should make them "private set" and use attributes
    //
    [DataContract]
    public class CreateOrderCommand:IRequest<bool>
    {
        [DataMember]
        private readonly List<OrderItemDto> orderItems;
        [DataMember]
        public IEnumerable<OrderItemDto> OrderItems { get { return orderItems; } }
        [DataMember]
        public string UserId { get; private set; }
        [DataMember]
        public string UserName { get; private set; }
        [DataMember]
        public string City { get; private set; }
        [DataMember]
        public string State { get; private set; }
        [DataMember]
        public string Country { get; private set; }
        [DataMember]
        public string Street { get; private set; }
        [DataMember]
        public string ZipCode { get; private set; }

        public CreateOrderCommand()
        {
            orderItems = new List<OrderItemDto>();
        }
        public CreateOrderCommand(List<BasketItem> basketItems, string userId,
            string userName, string city, string state, string country,
            string street, string zipCode)
        {
            orderItems = basketItems.ToOrderItemDto().ToList();
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
        public int ProductId { get;  set; }
        public string ProductName { get;  set; }
        public int Quantity { get;  set; }
        public decimal UnitPrice { get;  set; }
        public decimal Discount { get; set; }
    }
}
