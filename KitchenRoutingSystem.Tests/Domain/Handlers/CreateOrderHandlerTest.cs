using AutoMapper;
using FluentAssertions;
using KitchenRoutingSystem.Domain.Commands.OrderCommands.Request;
using KitchenRoutingSystem.Domain.DTOs;
using KitchenRoutingSystem.Domain.Handlers.OrderHandlers;
using KitchenRoutingSystem.Domain.Services.Interfaces;
using KitchenRoutingSystem.Shared.Commands.Response;
using Moq;
using Xunit;

namespace KitchenRoutingSystem.Tests.Domain.Handlers.OrderHandlerTest
{
    public class CreateOrderHandlerTest
    {
        private Mock<IOrderPublishService> _orderPublishServiceMock;
        private readonly CreateOrderHandler _createOrderHandler;
        private Mock<IMapper> _mapperMock;

        public CreateOrderHandlerTest()
        {
            _orderPublishServiceMock = new Mock<IOrderPublishService>();
            _mapperMock = new Mock<IMapper>();
            _createOrderHandler = new CreateOrderHandler(_orderPublishServiceMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async void Handle_ShouldReturnCommandResponse()
        {
            _orderPublishServiceMock.Setup(a => a.PublishOrder(It.IsAny<CreateOrderRequest>())).Verifiable();

            var result = await _createOrderHandler.Handle(new OrderDto(), default);

            result.Should().BeOfType<CommandResponse>();
        }
    }
}
