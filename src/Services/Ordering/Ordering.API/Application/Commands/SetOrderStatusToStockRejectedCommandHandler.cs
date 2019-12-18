using MediatR;
using Ordering.Domain.AggregatesModel.OrderAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.API.Application.Commands
{
    public class SetOrderStatusToStockRejectedCommandHandler : IRequestHandler<SetOrderStatusToStockRejectedCommand>
    {
        private readonly IOrderRepository repository;

        public SetOrderStatusToStockRejectedCommandHandler(IOrderRepository repository)
        {
            this.repository = repository;
        }

        public async Task<Unit> Handle(SetOrderStatusToStockRejectedCommand request, CancellationToken cancellationToken)
        {
            var orderToBeUpdated=repository.Get(request.orderId);
            orderToBeUpdated.SetCancelledStatusWhenStockIsRejected();
            await repository.UnitOfWork.SaveChangesAsync();

            //just for mediateR
            return await Task.FromResult(Unit.Value);

        }
    }
}
