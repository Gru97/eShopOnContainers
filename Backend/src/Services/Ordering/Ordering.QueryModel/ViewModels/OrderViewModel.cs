using System;
using System.Collections.Generic;

namespace Ordering.QueryModel.ViewModels
{
    public class OrderViewModel
    {
        public int ordernumber { get; set; }
        public string buyername { get; set; }
        public DateTime date { get; set; }
        public string address { get; set; }
        public string description { get; set; }
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
