using KitchenRoutingSystem.Domain.Commands.OrderCommands.Request;
using KitchenRoutingSystem.Domain.DTOs;
using KitchenRoutingSystem.Shared.Commands.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace KitchenRoutingSystem.Api.Controllers
{
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrderController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("/Order")]       
        public async Task<ActionResult<CommandResponse>> Create([FromBody] OrderDto command
        )
        {
            var result = await _mediator.Send(command);

            return Ok(result);
        }
    }
}
