using FluentAssertions;
using KitchenRoutingSystem.Domain.Entities;
using KitchenRoutingSystem.Infra.Context;
using KitchenRoutingSystem.Infra.Repositories;
using System.Collections.Generic;
using Xunit;

namespace KitchenRoutingSystem.Tests.Infra.Repositories
{
    public class ProductRepositoryTests 
    {
        private ProductRepository _productRepository;
        private Context context;
        public ProductRepositoryTests()
        {
            context = new Context();
            _productRepository = new ProductRepository(context);         
        }

        [Fact]
        public void Get_ShouldReturnProductByIdAsync()
        {   
            var retorno = _productRepository.GetByIdAsync("1").Result;

            retorno.Should().BeOfType<Product>();
            retorno.Should().NotBeNull();
        }

        [Fact]
        public void GetAll_ShouldReturnAllProducts()
        {
            var retorno = _productRepository.GetAllAsync().Result;

            retorno.Should().BeOfType<List<Product>>();
            retorno.Should().NotBeNull();

        }

        [Fact]
        public void Add_ShouldReturnProduct()
        {
            var newProduct = new Product
            {
                ProductId = "2",
                ProductType = KitchenRoutingSystem.Domain.Enums.EProductType.Grill,
                Quantity = 1,
                Status = KitchenRoutingSystem.Domain.Enums.EProductStatus.Pending,
                Value = 1
            };

            var retorno = _productRepository.UpdateAsync(newProduct).Result;

            retorno.Should().BeOfType<Product>();
            retorno.Should().NotBeNull();

        }

        [Fact]
        public async void Edit_ShouldReturnOrder()
        {
            var product = _productRepository.GetByIdAsync("1").Result;

            product.Quantity = 2;

            await _productRepository.UpdateAsync(product);
            product.Quantity.Should().Equals(2);
           
        }

        [Fact]
        public async void Delete_ShouldReturnEmptyProduct()
        {
            await _productRepository.RemoveAsync("1");

            var productEmpty = _productRepository.GetByIdAsync("1").Result;

            productEmpty.Should().BeNull();
        }
    }
}
