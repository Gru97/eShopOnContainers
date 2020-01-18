using System;
using System.Collections.Generic;
using System.Text;

namespace Ordering.Domain.AggregatesModel.OrderAggregate
{
    public enum OrderState
    {
        Submitted=0,
        AwaitingValidation=1,
        StockConfirmed=2,
        Paid=3,
        Shipped=4,
        Cancelled=5
    }
}
