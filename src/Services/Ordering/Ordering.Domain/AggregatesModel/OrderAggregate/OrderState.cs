using System;
using System.Collections.Generic;
using System.Text;

namespace Ordering.Domain.AggregatesModel.OrderAggregate
{
    public enum OrderState
    {
        Submitted,
        AwaitingValidation,
        StockConfirmed,
        Paid,
        Shipped,
        Cancelled
    }
}
