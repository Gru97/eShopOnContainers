using System.Collections.Generic;
using System.Linq;
using Ordering.Application.Commands;

namespace Ordering.Application.Models
{
    public static class BasketItemExtension
    {
        public static IEnumerable< OrderItemDto> ToOrderItemDto(this IEnumerable< BasketItem> items)
        {
            return items.Select(e => new OrderItemDto {ProductId=e.ProductId,
                ProductName=e.ProductName,
                Quantity=e.Quantity,
                UnitPrice=e.UnitPrice,
                });
        }
    }
}
