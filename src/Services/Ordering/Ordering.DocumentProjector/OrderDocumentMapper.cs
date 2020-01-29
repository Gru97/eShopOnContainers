using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ordering.Domain.AggregatesModel.OrderAggregate;
using Ordering.QueryModel;

namespace Ordering.DocumentProjector
{
    public static  class OrderDocumentMapper
    {

        public static OrderDocument ToOrderDocumant(this Order o)
        {
            return new OrderDocument()
            {
                OrderDate = o.OrderDate,
                OrderId = o.Id,
                Address = o.Address.ToAddressDocument(),
                Status = o.OrderState.ToString(),
                OrderItems = o.OrderItems.ToOrderItemDocumenList(),
            
                
            };

        }

        public static List<QueryModel.OrderItem> ToOrderItemDocumenList(this IReadOnlyCollection<Domain.AggregatesModel.OrderAggregate.OrderItem> o)
        {
            return o.Select(ToOrderItemDocument).ToList();


        }
        public static QueryModel.OrderItem ToOrderItemDocument(Domain.AggregatesModel.OrderAggregate.OrderItem o)
        {
            return new QueryModel.OrderItem()
            {
                Quantity = o.Quantity,
                ProductId = o.ProductId,
                Discount = o.Discount,
                UnitPrice = o.UnitPrice,
                ProductName = o.ProductName
            };
        }
        public  static QueryModel.Address ToAddressDocument(this  Domain.AggregatesModel.OrderAggregate.Address address)
        {
           return new QueryModel.Address()
           {
               State = address.State,
               City = address.City,
               Country = address.Country,
               ZipCode = address.ZipCode,
               Street = address.Street,
           };
        }
        public static Buyer ToBuyerDocument(this Domain.AggregatesModel.BuyerAggregate.Buyer buyer)
        {
            return new Buyer()
            {
                BuyerId = buyer.IdentityGuid,
                BuyerName = buyer.Name
            };
        }
    }
}
