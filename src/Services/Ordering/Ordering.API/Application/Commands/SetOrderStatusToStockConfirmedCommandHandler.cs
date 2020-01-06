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
    public class SetOrderStatusToStockConfirmedCommandHandler : IRequestHandler<SetOrderStatusToStockConfirmedCommand>
    {
        private readonly IOrderRepository repository;
        private readonly ILogger<SetOrderStatusToStockConfirmedCommandHandler> logger;

        public SetOrderStatusToStockConfirmedCommandHandler(IOrderRepository repository,
            ILogger<SetOrderStatusToStockConfirmedCommandHandler> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        public async Task<Unit> Handle(SetOrderStatusToStockConfirmedCommand request, CancellationToken cancellationToken)
        {
            var orderToBeUpdated = repository.Get(request.orderId);
            logger.LogInformation("Order retrieved {@orderToBeUpdated}", orderToBeUpdated);

            orderToBeUpdated.SetStockConfirmedStatus();
            logger.LogInformation("SetStockConfirmedStatus called on order object");

            await repository.UnitOfWork.SaveChangesAsync();
            logger.LogInformation("SaveChanges finished ");

            //raise an integration event so basket would listen to and remove items
            //But we don't have basket key (Id) for this user. It's not in the events
            return await Task.FromResult(Unit.Value);
        }
    }
}
