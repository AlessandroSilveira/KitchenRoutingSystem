using AutoMapper;
using FluentAssertions;
using KitchenRoutingSystem.Domain.DTOs;
using KitchenRoutingSystem.Domain.Entities;
using KitchenRoutingSystem.Domain.Repositories;
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
        private Mock<IRepository<Order>> _repositoryOrderMock;
        private Mock<IRepository<Product>> _repositoryProductMock;
        private Mock<ILogger<PrepareGrillHandler>> _loggerMock;
        private Mock<IMapper> _mapperMock;
        private PrepareGrillHandler prepareGrillHandler;
        private readonly ProductRepository _productRepository;
        private readonly OrderRepository _orderRepository;
        Order order;
        Product product;

        public PrepareSaladHandlerTests()
        {
            _repositoryProductMock = new Mock<IRepository<Product>>();
            _repositoryOrderMock = new Mock<IRepository<Order>>();
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
            }, "Teste");

            product = new Product
            {
                ProductId = "1",
                ProductType = KitchenRoutingSystem.Domain.Enums.EProductType.Grill,
                Quantity = 1,
                Status = KitchenRoutingSystem.Domain.Enums.EProductStatus.Pending,
                Value = 1
            };
            _productRepository = new ProductRepository();
            _orderRepository = new OrderRepository();

        }

        [Fact]
        public void Handler_ShouldReturnCommandResponse()
        {
            var orderAdd = _orderRepository.Add(order).Result;
            var productAdd = _productRepository.Add(product).Result;
            var productList = _productRepository.GetAll().Result;

            var prepareGrillRequest = new PrepareGrillRequest
            {
                orderId = orderAdd.Number,
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

            _repositoryProductMock.Setup(a => a.GetAll()).ReturnsAsync(productList);
            _repositoryOrderMock.Setup(a => a.Get(orderAdd.Number)).ReturnsAsync(orderAdd);
            _repositoryOrderMock.Setup(a => a.Edit(orderAdd)).Verifiable();
            _repositoryProductMock.Setup(a => a.Edit(productAdd)).Verifiable();
            _mapperMock.Setup(a => a.Map<List<ProductDto>>(orderAdd.Products)).Returns(prepareGrillRequest.products);

            var result = prepareGrillHandler.Handle(prepareGrillRequest, default).Result;

            result.Should().BeOfType<CommandResponse>();

        }

    }
}
