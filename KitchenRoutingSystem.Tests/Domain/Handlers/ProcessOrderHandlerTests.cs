using AutoMapper;
using FluentAssertions;
using KitchenRoutingSystem.Domain.Commands.PorcessOrderCommands.Request;
using KitchenRoutingSystem.Domain.Entities;
using KitchenRoutingSystem.Domain.Handlers.ProcessOrderHandlers;
using KitchenRoutingSystem.Domain.Repository;
using KitchenRoutingSystem.Shared.Commands.Response;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace KitchenRoutingSystem.Tests.Domain.Handlers
{
    public class ProcessOrderHandlerTests
    {
        private Mock<IOrderRepository> _repositoryMock;
        private Mock<ILogger<ProcessOrderHandler>> _loggerMock;
        private Mock<IMediator> _mediatorMock;
        private Mock<IMapper> _mapperMock;
        private  ProcessOrderHandler _processOrderHandler;

        public ProcessOrderHandlerTests()
        {
            _repositoryMock = new Mock<IOrderRepository>();
            _loggerMock = new Mock<ILogger<ProcessOrderHandler>>();
            _mapperMock = new Mock<IMapper>();
            _mediatorMock = new Mock<IMediator>();
            _processOrderHandler = new ProcessOrderHandler(_repositoryMock.Object, _mediatorMock.Object, _loggerMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async void Handle_ShouldReturnCommandResponse()
        {
            _repositoryMock.Setup(a => a.AddAsync(It.IsAny<Order>())).ReturnsAsync(It.IsAny<Order>());

            var result = await _processOrderHandler.Handle(new ProcessOrderRequest(), default);

            result.Should().BeOfType<CommandResponse>();
        }
    }
}
