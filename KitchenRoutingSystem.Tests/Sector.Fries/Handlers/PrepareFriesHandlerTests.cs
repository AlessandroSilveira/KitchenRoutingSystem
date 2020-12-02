using AutoMapper;
using FluentAssertions;
using KitchenRoutingSystem.Domain.DTOs;
using KitchenRoutingSystem.Domain.Entities;
using KitchenRoutingSystem.Domain.Repositories;
using KitchenRoutingSystem.Infra.Repositories;
using KitchenRoutingSystem.Sector.Fries.Commands.Request;
using KitchenRoutingSystem.Sector.Fries.Handlers.PrepareFriesHandler;
using KitchenRoutingSystem.Shared.Commands.Response;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace KitchenRoutingSystem.Tests.Sector.Fries.Handlers
{
    public class PrepareGrillHandlerTests
    {
        private Mock<IRepository<Order>> _repositoryOrderMock;
        private Mock<IRepository<Product>> _repositoryProductMock;
        private Mock<ILogger<PrepareFriesHandler>> _loggerMock;
        private Mock<IMapper> _mapperMock;
        private PrepareFriesHandler prepareFriesHandler;
        private readonly ProductRepository _productRepository;
        private readonly OrderRepository _orderRepository;
        Order order;
        Product product;

        public PrepareGrillHandlerTests()
        {
            _repositoryProductMock = new Mock<IRepository<Product>>();
            _repositoryOrderMock = new Mock<IRepository<Order>>();
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
            _productRepository = new ProductRepository();
            _orderRepository = new OrderRepository();

        }

        [Fact]
        public void Handler_ShouldReturnCommandResponse()
        {
            var orderAdd = _orderRepository.Add(order).Result;
            var productAdd = _productRepository.Add(product).Result;
            var productList = _productRepository.GetAll().Result;

            var prepareFriesRequest = new PrepareFriesRequest
            {
                orderId = orderAdd.Number,
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

            _repositoryProductMock.Setup(a => a.GetAll()).ReturnsAsync(productList);
            _repositoryOrderMock.Setup(a => a.Get(orderAdd.Number)).ReturnsAsync(orderAdd);
            _repositoryOrderMock.Setup(a => a.Edit(orderAdd)).Verifiable();
            _repositoryProductMock.Setup(a => a.Edit(productAdd)).Verifiable();
            _mapperMock.Setup(a => a.Map<List<ProductDto>>(orderAdd.Products)).Returns(prepareFriesRequest.products);

            var result = prepareFriesHandler.Handle(prepareFriesRequest, default).Result;

            result.Should().BeOfType<CommandResponse>();

        }

    }
}
