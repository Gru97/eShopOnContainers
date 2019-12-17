 using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ordering.API.Application.Commands;
using Ordering.API.Application.Queries;

namespace Ordering.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private IMediator mediator;

        public OrderController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [Route("order")]
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

        [Route("order/{userId}")]
        [HttpGet]
        public async Task<ActionResult> GetOrderForUser(string userId)
        {
            var orders = await mediator.Send(new GetOrdersForBuyerQuery(userId));
            if (orders == null || orders.Count < 1)
                return NoContent();

            return Ok(orders);
        }

        [Route("order")]
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var orders =await mediator.Send(new GetAllOrdersQuery());
            if (orders == null || orders.Count < 1)
                return NoContent();

            return Ok(orders);
        }



    }
}
