using AutoMapper;
using FluentAssertions;
using KitchenRoutingSystem.Domain.DTOs;
using KitchenRoutingSystem.Domain.Entities;
using KitchenRoutingSystem.Domain.Repository;
using KitchenRoutingSystem.Sector.Fries.Commands.Request;
using KitchenRoutingSystem.Sector.Fries.Handlers.PrepareFriesHandler;
using KitchenRoutingSystem.Shared.Commands.Response;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace KitchenRoutingSystem.Tests.Sector.Fries.Handlers
{
    public class PrepareGrillHandlerTests
    {
        private Mock<IOrderRepository> _repositoryOrderMock;
        private Mock<IProductRepository> _repositoryProductMock;
        private Mock<ILogger<PrepareFriesHandler>> _loggerMock;
        private Mock<IMapper> _mapperMock;
        private PrepareFriesHandler prepareFriesHandler;
       
        Order order;
        Product product;

        public PrepareGrillHandlerTests()
        {
            _repositoryProductMock = new Mock<IProductRepository>();
            _repositoryOrderMock = new Mock<IOrderRepository>();
            _loggerMock = new Mock<ILogger<PrepareFriesHandler>>();
            _mapperMock = new Mock<IMapper>();
            prepareFriesHandler = new PrepareFriesHandler(_repositoryProductMock.Object, _loggerMock.Object, _repositoryOrderMock.Object, _mapperMock.Object);

            order = new Order(new List<Product>
            {
                new Product
                {
                    ProductId = "1",
                    ProductType = KitchenRoutingSystem.Domain.Enums.EProductType.Fries,
                    Quantity = 1,
                    Status = KitchenRoutingSystem.Domain.Enums.EProductStatus.Pending,
                    Value = 1
                }
            });

            product = new Product
            {
                ProductId = "1",
                ProductType = KitchenRoutingSystem.Domain.Enums.EProductType.Fries,
                Quantity = 1,
                Status = KitchenRoutingSystem.Domain.Enums.EProductStatus.Pending,
                Value = 1
            };
           

        }

        [Fact]
        public void Handler_ShouldReturnCommandResponse()
        {
           

            var prepareFriesRequest = new PrepareFriesRequest
            {
                orderId = order.Number,
                products = new List<ProductDto>
                {
                    new ProductDto
                    {
                        ProductId="1",
                        ProductType = KitchenRoutingSystem.Domain.Enums.EProductType.Fries,
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
            _mapperMock.Setup(a => a.Map<List<ProductDto>>(order.Products)).Returns(prepareFriesRequest.products);

            var result = prepareFriesHandler.Handle(prepareFriesRequest, default).Result;

            result.Should().BeOfType<CommandResponse>();

        }

    }
}
