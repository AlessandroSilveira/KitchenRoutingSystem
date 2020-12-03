using AutoMapper;
using FluentAssertions;
using KitchenRoutingSystem.Domain.DTOs;
using KitchenRoutingSystem.Domain.Entities;
using KitchenRoutingSystem.Domain.Repository;
using KitchenRoutingSystem.Sector.Dessert.Handlers.PrepareDessertHandler;
using KitchenRoutingSystem.Sector.Desserts.Commands.Request;
using KitchenRoutingSystem.Shared.Commands.Response;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace KitchenRoutingSystem.Tests.Sector.Dessert.Handlers
{
    public class PrepareDessertHandlerTests
    {
        private Mock<IOrderRepository> _repositoryOrderMock;
        private Mock<IProductRepository> _repositoryProductMock;
        private Mock<ILogger<PrepareDessertHandler>> _loggerMock;
        private Mock<IMapper> _mapperMock;
        private PrepareDessertHandler prepareDessertHandler;       
        Order order;
        Product product;

        public PrepareDessertHandlerTests()
        {
            _repositoryProductMock = new Mock<IProductRepository>();
            _repositoryOrderMock = new Mock<IOrderRepository>();
            _loggerMock = new Mock<ILogger<PrepareDessertHandler>>();
            _mapperMock = new Mock<IMapper>();
            prepareDessertHandler = new PrepareDessertHandler(_repositoryProductMock.Object, _loggerMock.Object, _repositoryOrderMock.Object, _mapperMock.Object);

            order = new Order(new List<Product>
            {
                new Product
                {
                    ProductId = "1",
                    ProductType = KitchenRoutingSystem.Domain.Enums.EProductType.Dessert,
                    Quantity = 1,
                    Status = KitchenRoutingSystem.Domain.Enums.EProductStatus.Pending,
                    Value = 1
                }
            });

            product = new Product
            {
                ProductId = "1",
                ProductType = KitchenRoutingSystem.Domain.Enums.EProductType.Dessert,
                Quantity = 1,
                Status = KitchenRoutingSystem.Domain.Enums.EProductStatus.Pending,
                Value = 1
            };
        }

        [Fact]
        public async void Handler_ShouldReturnCommandResponse()
        {
            var prepareDessertRequest = new PrepareDessertRequest
            {
                orderId = order.Number,
                products = new List<ProductDto>
                {
                    new ProductDto
                    {
                        ProductId="1",
                        ProductType = KitchenRoutingSystem.Domain.Enums.EProductType.Dessert,
                        Quantity = 1,
                        Status = KitchenRoutingSystem.Domain.Enums.EProductStatus.Pending,
                        Value = 1
                    }
                }
            };

            _repositoryOrderMock.Setup(a => a.GetByIdAsync(order.Number)).ReturnsAsync(order);
            _repositoryOrderMock.Setup(a => a.UpdateAsync(order)).ReturnsAsync(order);
            _repositoryProductMock.Setup(a => a.UpdateAsync(product)).ReturnsAsync(product);

            _mapperMock.Setup(a => a.Map<List<ProductDto>>(order.Products)).Returns(prepareDessertRequest.products);

            var result = await prepareDessertHandler.Handle(prepareDessertRequest, default);

            result.Should().BeOfType<CommandResponse>();

        }

    }
}
