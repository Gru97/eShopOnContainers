using System;
using System.Collections.Generic;
using System.Text;

namespace Ordering.Domain.AggregatesModel.OrderAggregate
{
    public class PagedResult<T>
    {
        public List<T> Items { get; set; }
        public int Count { get; set; }

        public PagedResult()
        {
            Items = new List<T>();
        }
    }
}
