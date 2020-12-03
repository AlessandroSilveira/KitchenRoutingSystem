using AutoMapper;
using FluentAssertions;
using KitchenRoutingSystem.Domain.DTOs;
using KitchenRoutingSystem.Domain.Entities;
using KitchenRoutingSystem.Domain.Repository;
using KitchenRoutingSystem.Infra.Repositories;
using KitchenRoutingSystem.Sector.Grill.Commands.Request;
using KitchenRoutingSystem.Sector.Grill.Handlers.PrepareGrillHandler;
using KitchenRoutingSystem.Shared.Commands.Response;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace KitchenRoutingSystem.Tests.Sector.Grill.Handlers
{
    public class PrepareSaladHandlerTests
    {
        private Mock<IOrderRepository> _repositoryOrderMock;
        private Mock<IProductRepository> _repositoryProductMock;
        private Mock<ILogger<PrepareGrillHandler>> _loggerMock;
        private Mock<IMapper> _mapperMock;
        private PrepareGrillHandler prepareGrillHandler;        
        Order order;
        Product product;

        public PrepareSaladHandlerTests()
        {
            _repositoryProductMock = new Mock<IProductRepository>();
            _repositoryOrderMock = new Mock<IOrderRepository>();
            _loggerMock = new Mock<ILogger<PrepareGrillHandler>>();
            _mapperMock = new Mock<IMapper>();
            prepareGrillHandler = new PrepareGrillHandler(_repositoryProductMock.Object, _loggerMock.Object, _repositoryOrderMock.Object, _mapperMock.Object);

            order = new Order(new List<Product>
            {
                new Product
                {
                    ProductId = "1",
                    ProductType = KitchenRoutingSystem.Domain.Enums.EProductType.Grill,
                    Quantity = 1,
                    Status = KitchenRoutingSystem.Domain.Enums.EProductStatus.Pending,
                    Value = 1
                }
            });

            product = new Product
            {
                ProductId = "1",
                ProductType = KitchenRoutingSystem.Domain.Enums.EProductType.Grill,
                Quantity = 1,
                Status = KitchenRoutingSystem.Domain.Enums.EProductStatus.Pending,
                Value = 1
            };
            

        }

        [Fact]
        public void Handler_ShouldReturnCommandResponse()
        {
         

            var prepareGrillRequest = new PrepareGrillRequest
            {
                orderId = order.Number,
                products = new List<ProductDto>
                {
                    new ProductDto
                    {
                        ProductId="1",
                        ProductType = KitchenRoutingSystem.Domain.Enums.EProductType.Grill,
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
            _mapperMock.Setup(a => a.Map<List<ProductDto>>(order.Products)).Returns(prepareGrillRequest.products);

            var result = prepareGrillHandler.Handle(prepareGrillRequest, default).Result;

            result.Should().BeOfType<CommandResponse>();

        }

    }
}
