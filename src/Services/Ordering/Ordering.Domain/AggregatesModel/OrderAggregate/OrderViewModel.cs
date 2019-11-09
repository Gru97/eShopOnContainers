using System;
using System.Collections.Generic;
using System.Text;

namespace Ordering.Domain.AggregatesModel.OrderAggregate
{
    public class OrderViewModel
    {
        public int ordernumber { get; set; }
        public DateTime date { get; set; }
        public string status { get; set; }
        public string description { get; set; }
        public string street { get; set; }
        public string city { get; set; }
        public string zipcode { get; set; }
        public string country { get; set; }
        public List<OrderItemViewModel> orderitems { get; set; }
        public decimal total { get; set; }
    }
    public class OrderItemViewModel
    {
        public string productname { get; set; }
        public int units { get; set; }
        public decimal unitprice { get; set; }
    }
    public class OrderSummeryViewModel
    {
        public int ordernumber { get; set; }
        public DateTime date { get; set; }
        public string status { get; set; }
        public decimal total { get; set; }
    }
}
