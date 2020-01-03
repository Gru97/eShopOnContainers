using MediatR;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<SetOrderStatusToStockRejectedCommandHandler> logger;

        public SetOrderStatusToStockRejectedCommandHandler(IOrderRepository repository,
            ILogger<SetOrderStatusToStockRejectedCommandHandler> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        public async Task<Unit> Handle(SetOrderStatusToStockRejectedCommand request, CancellationToken cancellationToken)
        {
            var orderToBeUpdated=repository.Get(request.orderId);
            logger.LogInformation("order retrieved to be set as rejected. {@orderToBeUpdated}", orderToBeUpdated);

            orderToBeUpdated.SetCancelledStatusWhenStockIsRejected();
            logger.LogInformation("order status changed to rejected");

            await repository.UnitOfWork.SaveChangesAsync();
            logger.LogInformation("SaveChanges called for order");

            //just for mediateR
            return await Task.FromResult(Unit.Value);

        }
    }
}
