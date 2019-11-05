using Ordering.API.Application.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ordering.API.Application.Models
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
