using FluentAssertions;
using KitchenRoutingSystem.Domain.Entities;
using KitchenRoutingSystem.Infra.Repositories;
using System.Collections.Generic;
using Xunit;

namespace KitchenRoutingSystem.Tests.Infra.Repositories
{
    public class OrderRepositoryTests
    {
       
        private OrderRepository _orderRepository;
        Order order;

        public OrderRepositoryTests()
        {  
            _orderRepository = new OrderRepository();
            var listProduct = new List<Product>
            {
                new Product
                {
                    ProductId = "1",
                    ProductType = KitchenRoutingSystem.Domain.Enums.EProductType.Dessert,
                    Quantity = 1,
                    Status = KitchenRoutingSystem.Domain.Enums.EProductStatus.Delivered,
                    Value = 1
                }
            };
            order = new Order(listProduct);
        }

        [Fact]
        public void Get_ShouldReturnOrderByIdAsync()
        {
            var orderAdd = _orderRepository.Add(order).Result;
            var retorno = _orderRepository.Get(orderAdd.Number).Result;
           
            retorno.Should().BeOfType<Order>();
            retorno.Should().NotBeNull();
            
        }

        [Fact]
        public void GetAll_ShouldReturnAllOrder()
        {
            var orderAdd = _orderRepository.Add(order).Result;

            var retorno = _orderRepository.GetAll().Result;

            retorno.Should().BeOfType<List<Order>>();
            retorno.Should().NotBeNull();

        }

        [Fact]
        public void Add_ShouldReturnOrder()
        {
            var retorno = _orderRepository.Add(order).Result;

            retorno.Should().BeOfType<Order>();
            retorno.Should().NotBeNull();

        }

        [Fact]
        public async void Edit_ShouldReturnOrder()
        {
            var orderAdd = _orderRepository.Add(order).Result;

            var newProduct = new Product
            {
                ProductId = "2",
                ProductType = KitchenRoutingSystem.Domain.Enums.EProductType.Grill,
                Quantity = 1,
                Status = KitchenRoutingSystem.Domain.Enums.EProductStatus.Pending,
                Value = 1
            };

            orderAdd.Products.Add(newProduct);

            await _orderRepository.Edit(orderAdd);

            var getOrder = _orderRepository.Get(orderAdd.Number);

            getOrder.Result.Products.Count.Should().BeGreaterThan(1);
            getOrder.Result.Should().NotBeNull();
        }

        [Fact]
        public async void Delete_ShouldReturnEmptyOrder()
        {
            var retorno = _orderRepository.Add(order).Result;

            await _orderRepository.Delete(retorno.Number);

            var orderEmpty = _orderRepository.Get(retorno.Number).Result;

            orderEmpty.Should().BeNull();
        }
    }
}
