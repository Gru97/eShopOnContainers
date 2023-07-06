using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Ordering.QueryModel

{
    public class OrderDocument
    {
        public static string DocumentName = "orders";

        [BsonId]
        public int OrderId { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public DateTime OrderDate { get; set; }
        public Address Address { get; set; }
        
        public Buyer BuyerInfo { get; set; }
        public List<OrderItem> OrderItems { get; set; }

        public OrderDocument()
        {
            BuyerInfo=new Buyer();
            Address=new Address();
        }
    }
    
}
