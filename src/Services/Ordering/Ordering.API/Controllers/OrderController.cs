 using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ordering.API.Application.Commands;

namespace Ordering.API.Controllers
{
    [Authorize]
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
        // GET api/values
        [HttpGet]
        public ActionResult<string> Get(int id)
        {
            return new JsonResult(from c in User.Claims select new { c.Type, c.Value });
           
        }

       


    }
}
