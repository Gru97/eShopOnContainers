 using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Ordering.API.Application.Commands;
using Ordering.API.Application.Queries;

namespace Ordering.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private IMediator mediator;
        private ILogger<OrderController> logger;
        public OrderController(IMediator mediator, ILogger<OrderController> logger)
        {
            this.mediator = mediator;
            this.logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult> Order()
        {
            var cmd = new CreateOrderCommand(new List<Application.Models.BasketItem>(),
                "1","name","city","state","country","street","zipcode");

            var result=mediator.Send(cmd);
            if (result.Result)
                return Accepted();
            else
                return BadRequest();
        }

        [HttpGet]
        [Route("BuyerId/{buyerId}")]
        public async Task<ActionResult> GetOrderForUser( string buyerId)
        {
            var orders = await mediator.Send(new GetOrdersForBuyerQuery(buyerId));
            if (orders == null || orders.Count < 1)
                return NoContent();

            return Ok(orders);
        }

        [HttpGet]
        [Route("all")]
        public async Task<ActionResult> GetAll([FromQuery] int pageSize = 2, [FromQuery] int pageIndex = 0)
        {
            var orders =await mediator.Send(new GetAllOrdersQuery(pageSize,pageIndex));
            if (orders == null || orders.Count < 1)
                return NoContent();
            logger.LogInformation("GetAll action inside Order Controller");
            return Ok(orders);
        }

        [HttpGet]
        [Route("OrderId/{orderId}")]
        public async Task<ActionResult> GetByOrderId( int orderId,[FromQuery] int pageSize=2, [FromQuery] int pageIndex=0)
        {
            var order = await mediator.Send(new GetOrderByIdQuery(orderId));
          

            return Ok(order);
        }

        [Route("status/{status}")]
        [HttpGet]
        public async Task<ActionResult> GetAllByStatus(int status, [FromQuery] int pageSize = 2, [FromQuery] int pageIndex = 0)
        {
            var orders = await mediator.Send(new GetOrdersByStatusQuery(pageSize, pageIndex, status));
            if (orders == null || orders.Count < 1)
                return NoContent();

            return Ok(orders);
        }

        [HttpGet]
        [Route("details/{orderId}")]
        public async Task<ActionResult> GetOrderDetailsByOrderId(int orderId)
        {
            var order = await mediator.Send(new GetOrderDetailsQuery(orderId));
                return Ok(order);
        }

    }
}
