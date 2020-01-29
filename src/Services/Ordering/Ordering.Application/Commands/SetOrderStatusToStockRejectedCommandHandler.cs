using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Domain.AggregatesModel.OrderAggregate;

namespace Ordering.Application.Commands
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
            var orderToBeUpdated=await repository.FindAsycn(request.orderId);
            logger.LogInformation($"order retrieved to be set as rejected. {@orderToBeUpdated}", orderToBeUpdated);

            if (orderToBeUpdated.Id == default)
            {
                logger.LogInformation("Id is not valid for orderToBeUpdated to be updated!");

            }
            else
            {

                orderToBeUpdated.SetCancelledStatusWhenStockIsRejected();
                logger.LogInformation("order status changed to rejected");

                await repository.UnitOfWork.SaveChangesAsync();
                logger.LogInformation("SaveChanges called for order");

            }


            //just for mediateR
            return await Task.FromResult(Unit.Value);

        }
    }
}
