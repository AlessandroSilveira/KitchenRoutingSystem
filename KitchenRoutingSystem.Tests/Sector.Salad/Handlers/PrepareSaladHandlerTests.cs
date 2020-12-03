using AutoMapper;
using FluentAssertions;
using KitchenRoutingSystem.Domain.DTOs;
using KitchenRoutingSystem.Domain.Entities;
using KitchenRoutingSystem.Domain.Repository;
using KitchenRoutingSystem.Sector.Salad.Commands.Request;
using KitchenRoutingSystem.Sector.Salad.Handlers.PrepareSaladHandler;
using KitchenRoutingSystem.Shared.Commands.Response;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace KitchenRoutingSystem.Tests.Sector.Salad.Handlers
{
    public class PrepareSaladHandlerTests
    {
        private Mock<IOrderRepository> _repositoryOrderMock;
        private Mock<IProductRepository> _repositoryProductMock;
        private Mock<ILogger<PrepareSaladHandler>> _loggerMock;
        private Mock<IMapper> _mapperMock;
        private PrepareSaladHandler prepareSaladHandler;
        Order order;
        Product product;

        public PrepareSaladHandlerTests()
        {
            _repositoryProductMock = new Mock<IProductRepository>();
            _repositoryOrderMock = new Mock<IOrderRepository>();
            _loggerMock = new Mock<ILogger<PrepareSaladHandler>>();
            _mapperMock = new Mock<IMapper>();
            prepareSaladHandler = new PrepareSaladHandler(_repositoryProductMock.Object, _loggerMock.Object, _repositoryOrderMock.Object, _mapperMock.Object);

            order = new Order(new List<Product>
            {
                new Product
                {
                    ProductId = "1",
                    ProductType = KitchenRoutingSystem.Domain.Enums.EProductType.Salad,
                    Quantity = 1,
                    Status = KitchenRoutingSystem.Domain.Enums.EProductStatus.Pending,
                    Value = 1
                }
            });

            product = new Product
            {
                ProductId = "1",
                ProductType = KitchenRoutingSystem.Domain.Enums.EProductType.Salad,
                Quantity = 1,
                Status = KitchenRoutingSystem.Domain.Enums.EProductStatus.Pending,
                Value = 1
            };
         

        }

        [Fact]
        public async void Handler_ShouldReturnCommandResponse()
        {
            var prepareSaladRequest = new PrepareSaladRequest
            {
                orderId = order.Number,
                products = new List<ProductDto>
                {
                    new ProductDto
                    {
                        ProductId="1",
                        ProductType = KitchenRoutingSystem.Domain.Enums.EProductType.Salad,
                        Quantity = 1,
                        Status = KitchenRoutingSystem.Domain.Enums.EProductStatus.Pending,
                        Value = 1
                    }
                }
            };

            _repositoryProductMock.Setup(a => a.GetAllAsync()).ReturnsAsync(It.IsAny<List<Product>>);
            _repositoryOrderMock.Setup(a => a.GetByIdAsync(order.Number)).ReturnsAsync(order);
            _repositoryOrderMock.Setup(a => a.UpdateAsync(order)).Verifiable();
            _repositoryProductMock.Setup(a => a.UpdateAsync(product)).Verifiable();
            _mapperMock.Setup(a => a.Map<List<ProductDto>>(order.Products)).Returns(prepareSaladRequest.products);

            var result = await prepareSaladHandler.Handle(prepareSaladRequest, default);

            result.Should().BeOfType<CommandResponse>();

        }

    }
}
