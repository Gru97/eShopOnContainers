using MediatR;
using Ordering.Domain.AggregatesModel.OrderAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ordering.API.Application.Queries
{
    public class GetAllOrdersQuery:IRequest<List<OrderViewModel>>
    {
    }
}
