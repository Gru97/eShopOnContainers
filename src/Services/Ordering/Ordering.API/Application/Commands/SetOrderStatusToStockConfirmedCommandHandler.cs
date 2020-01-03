using MediatR;
using Ordering.Domain.AggregatesModel.OrderAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.API.Application.Commands
{
    public class SetOrderStatusToStockConfirmedCommandHandler : IRequestHandler<SetOrderStatusToStockConfirmedCommand>
    {
        private readonly IOrderRepository repository;

        public SetOrderStatusToStockConfirmedCommandHandler(IOrderRepository repository)
        {
            this.repository = repository;
        }

        public async Task<Unit> Handle(SetOrderStatusToStockConfirmedCommand request, CancellationToken cancellationToken)
        {
            var orderToBeUpdated = repository.Get(request.orderId);
            orderToBeUpdated.SetStockConfirmedStatus();
            await repository.UnitOfWork.SaveChangesAsync();
            
            return await Task.FromResult(Unit.Value);
        }
    }
}
