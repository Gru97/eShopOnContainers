using System.Collections.Generic;

namespace Ordering.QueryModel.ViewModels
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
