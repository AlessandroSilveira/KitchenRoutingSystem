using FluentAssertions;
using KitchenRoutingSystem.Domain.Entities;
using KitchenRoutingSystem.Shared.Commands.Response;
using KitchenRoutingSystem.Shared.Handler;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace KitchenRoutingSystem.Tests.Shared.Handler
{
    public class CommandHandlerTest
    {
        private readonly Mock<CommandHandler> _commandHandlerMock;
        private Order order;

        public CommandHandlerTest()
        {
            _commandHandlerMock = new Mock<CommandHandler>()
            {
                CallBase = true
            };

            order = new Order(new List<Product>
            {
                new Product()
            },
            "teste"); ;
        }

        [Fact]
        public void CreateResponse_Return_StatusCode_Created()
        {
            var commandResponse = new CommandResponse(order) 
            {
                StatusCode = 201,
                Message = "Created"
            };

            var result = _commandHandlerMock.Object.CreateResponse(order, commandResponse.Message);

            result.StatusCode.Should().Equals(commandResponse.StatusCode);
            result.Should().BeOfType<CommandResponse>();
            result.Message.Equals(commandResponse.Message);
        }

        [Fact]
        public void OKResponse_Return_StatusCode_OK()
        {
            var commandResponse = new CommandResponse(order)
            {
                StatusCode = 200,
                Message = "OK"
            };

            var result = _commandHandlerMock.Object.OkResponse(order, commandResponse.Message);

            result.StatusCode.Should().Equals(commandResponse.StatusCode);
            result.Should().BeOfType<CommandResponse>();
            result.Message.Equals(commandResponse.Message);
        }

        [Fact]
        public void BadRequestResponse_Return_StatusCode_BadRequest()
        {
            var commandResponse = new CommandResponse(order)
            {
                StatusCode = 400,
                Message = "BadRequest"
            };

            var result = _commandHandlerMock.Object.BadRequestResponse(order, commandResponse.Message);

            result.StatusCode.Should().Equals(commandResponse.StatusCode);
            result.Should().BeOfType<CommandResponse>();
            result.Message.Equals(commandResponse.Message);
        }

        [Fact]
        public void InternalServerErrorResponse_Return_StatusCode_InternalServerError()
        {
            var commandResponse = new CommandResponse(order)
            {
                StatusCode = 500,
                Message = "InternalServerError"
            };

            var result = _commandHandlerMock.Object.InternalServerErrorResponse(order, commandResponse.Message);

            result.StatusCode.Should().Equals(commandResponse.StatusCode);
            result.Should().BeOfType<CommandResponse>();
            result.Message.Equals(commandResponse.Message);
        }

    }
}
