//using FluentAssertions;
//using KitchenRoutingSystem.Domain.Commands.ProcessProductCommads;
//using KitchenRoutingSystem.Domain.Handlers.ProcessProductHandlers;
//using KitchenRoutingSystem.Domain.Services.Interfaces;
//using KitchenRoutingSystem.Shared.Commands.Response;
//using Moq;
//using System.Text;
//using Xunit;

//namespace KitchenRoutingSystem.Tests.Domain.Handlers
//{
//    public class ProcessProductHandlerTests
//    {
//        private Mock<IProcessProductService> _processProductServiceMock;
//        private ProcessProductHandler _processProductHandler;

//        public ProcessProductHandlerTests()
//        {
//            _processProductServiceMock = new Mock<IProcessProductService>();
//            _processProductHandler = new ProcessProductHandler(_processProductServiceMock.Object);
//        }

//        [Fact]
//        public async void Handle_WhithFriesProduct_ShouldReturnCommandResponse()
//        {
//            var processProductCommand = new ProcessProductCommad
//            {

//                orderId = "1",
//                products = new System.Collections.Generic.List<KitchenRoutingSystem.Domain.DTOs.ProductDto>
//                {
//                    new KitchenRoutingSystem.Domain.DTOs.ProductDto
//                    {
//                        ProductId="1",
//                        ProductType = KitchenRoutingSystem.Domain.Enums.EProductType.Fries,
//                        Quantity = 1,
//                        Status = KitchenRoutingSystem.Domain.Enums.EProductStatus.Pending,
//                        Value = 1
//                    }
//                }
//            };

//            var messageBodyBytes = Encoding.UTF8.GetBytes("teste");
//            _processProductServiceMock.Setup(a => a.SendOrderToFriesSector(messageBodyBytes)).Verifiable();

//            var result = await _processProductHandler.Handle(processProductCommand, default);

//            result.Should().BeOfType<CommandResponse>();
//        }
//        [Fact]
//        public async void Handle_WhithGrillProduct_ShouldReturnCommandResponse()
//        {
//            var processProductCommand = new ProcessProductCommad
//            {

//                orderId = "1",
//                products = new System.Collections.Generic.List<KitchenRoutingSystem.Domain.DTOs.ProductDto>
//                {
//                    new KitchenRoutingSystem.Domain.DTOs.ProductDto
//                    {
//                        ProductId="1",
//                        ProductType = KitchenRoutingSystem.Domain.Enums.EProductType.Grill,
//                        Quantity = 1,
//                        Status = KitchenRoutingSystem.Domain.Enums.EProductStatus.Pending,
//                        Value = 1
//                    }
//                }
//            };

//            var messageBodyBytes = Encoding.UTF8.GetBytes("teste");
//            _processProductServiceMock.Setup(a => a.SendOrderToGrillSector(messageBodyBytes)).Verifiable();

//            var result = await _processProductHandler.Handle(processProductCommand, default);

//            result.Should().BeOfType<CommandResponse>();
//        }
//        [Fact]
//        public async void Handle_WhithSaladProduct_ShouldReturnCommandResponse()
//        {
//            var processProductCommand = new ProcessProductCommad
//            {

//                orderId = "1",
//                products = new System.Collections.Generic.List<KitchenRoutingSystem.Domain.DTOs.ProductDto>
//                {
//                    new KitchenRoutingSystem.Domain.DTOs.ProductDto
//                    {
//                        ProductId="1",
//                        ProductType = KitchenRoutingSystem.Domain.Enums.EProductType.Salad,
//                        Quantity = 1,
//                        Status = KitchenRoutingSystem.Domain.Enums.EProductStatus.Pending,
//                        Value = 1
//                    }
//                }
//            };

//            var messageBodyBytes = Encoding.UTF8.GetBytes("teste");
//            _processProductServiceMock.Setup(a => a.SendOrderToSaladSector(messageBodyBytes)).Verifiable();

//            var result = await _processProductHandler.Handle(processProductCommand, default);

//            result.Should().BeOfType<CommandResponse>();
//        }
//        [Fact]
//        public async void Handle_WhithDrinksProduct_ShouldReturnCommandResponse()
//        {
//            var processProductCommand = new ProcessProductCommad
//            {

//                orderId = "1",
//                products = new System.Collections.Generic.List<KitchenRoutingSystem.Domain.DTOs.ProductDto>
//                {
//                    new KitchenRoutingSystem.Domain.DTOs.ProductDto
//                    {
//                        ProductId="1",
//                        ProductType = KitchenRoutingSystem.Domain.Enums.EProductType.Drink,
//                        Quantity = 1,
//                        Status = KitchenRoutingSystem.Domain.Enums.EProductStatus.Pending,
//                        Value = 1
//                    }
//                }
//            };

//            var messageBodyBytes = Encoding.UTF8.GetBytes("teste");
//            _processProductServiceMock.Setup(a => a.SendOrderToDrinkSector(messageBodyBytes)).Verifiable();

//            var result = await _processProductHandler.Handle(processProductCommand, default);

//            result.Should().BeOfType<CommandResponse>();
//        }

//        [Fact]
//        public async void Handle_WhithDessertProduct_ShouldReturnCommandResponse()
//        {
//            var processProductCommand = new ProcessProductCommad
//            {

//                orderId = "1",
//                products = new System.Collections.Generic.List<KitchenRoutingSystem.Domain.DTOs.ProductDto>
//                {
//                    new KitchenRoutingSystem.Domain.DTOs.ProductDto
//                    {
//                        ProductId="1",
//                        ProductType = KitchenRoutingSystem.Domain.Enums.EProductType.Dessert,
//                        Quantity = 1,
//                        Status = KitchenRoutingSystem.Domain.Enums.EProductStatus.Pending,
//                        Value = 1
//                    }
//                }
//            };

//            var messageBodyBytes = Encoding.UTF8.GetBytes("teste");
//            _processProductServiceMock.Setup(a => a.SendOrderToDessertSector(messageBodyBytes)).Verifiable();

//            var result = await _processProductHandler.Handle(processProductCommand, default);

//            result.Should().BeOfType<CommandResponse>();
//        }
//    }
//}
