using KitchenRoutingSystem.Domain.Commands.OrderCommands.Request;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult Create([FromBody] CreateOrderRequest command
        )
        {
            var dados = command;

            var result = _mediator.Send(command);
            return Ok(dados);
        }
    }
}
