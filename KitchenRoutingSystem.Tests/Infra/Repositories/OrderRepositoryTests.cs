//using FluentAssertions;
//using KitchenRoutingSystem.Domain.Entities;
//using KitchenRoutingSystem.Infra.Repositories;
//using System.Collections.Generic;
//using Xunit;

//namespace KitchenRoutingSystem.Tests.Infra.Repositories
//{
//    public class OrderRepositoryTests
//    {
       
//        private OrderRepository _orderRepository;
//        Order order;

//        public OrderRepositoryTests(OrderRepository orderRepository)
//        {

//            var listProduct = new List<Product>
//            {
//                new Product
//                {
//                    ProductId = "1",
//                    ProductType = KitchenRoutingSystem.Domain.Enums.EProductType.Dessert,
//                    Quantity = 1,
//                    Status = KitchenRoutingSystem.Domain.Enums.EProductStatus.Delivered,
//                    Value = 1
//                }
//            };
//            order = new Order(listProduct);
//            _orderRepository = orderRepository;
//        }

//        [Fact]
//        public void Get_ShouldReturnOrderByIdAsync()
//        {
//            var orderAdd = _orderRepository.AddAsync(order).Result;
//            var retorno = _orderRepository.GetByIdAsync(orderAdd.Number).Result;
           
//            retorno.Should().BeOfType<Order>();
//            retorno.Should().NotBeNull();            
//        }

//        [Fact]
//        public void GetAll_ShouldReturnAllOrder()
//        {
//            var orderAdd = _orderRepository.AddAsync(order).Result;

//            var retorno = _orderRepository.GetAllAsync().Result;

//            retorno.Should().BeOfType<List<Order>>();
//            retorno.Should().NotBeNull();
//        }

//        [Fact]
//        public void Add_ShouldReturnOrder()
//        {
//            var retorno = _orderRepository.AddAsync(order).Result;

//            retorno.Should().BeOfType<Order>();
//            retorno.Should().NotBeNull();

//        }

//        [Fact]
//        public async void Edit_ShouldReturnOrder()
//        {
//            var orderAdd = _orderRepository.AddAsync(order).Result;

//            var newProduct = new Product
//            {
//                ProductId = "2",
//                ProductType = KitchenRoutingSystem.Domain.Enums.EProductType.Grill,
//                Quantity = 1,
//                Status = KitchenRoutingSystem.Domain.Enums.EProductStatus.Pending,
//                Value = 1
//            };

//            orderAdd.Products.Add(newProduct);

//            await _orderRepository.UpdateAsync(orderAdd);

//            var getOrder = _orderRepository.GetByIdAsync(orderAdd.Number);

//            getOrder.Result.Products.Count.Should().BeGreaterThan(1);
//            getOrder.Result.Should().NotBeNull();
//        }

//        [Fact]
//        public async void Delete_ShouldReturnEmptyOrder()
//        {
//            var retorno = _orderRepository.AddAsync(order).Result;

//            await _orderRepository.RemoveAsync(retorno.Number);

//            var orderEmpty = _orderRepository.GetByIdAsync(retorno.Number).Result;

//            orderEmpty.Should().BeNull();
//        }
//    }
//}
