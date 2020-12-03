using KitchenRoutingSystem.Domain.Repository;
using KitchenRoutingSystem.Domain.Repository.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace KitchenRoutingSystem.Infra.Repositories.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        public UnitOfWork( IOrderRepository orderRepository, IProductRepository productRepository)
        {
            Orders = orderRepository;
            Products = productRepository;
        }

        public IOrderRepository Orders { get; }
        public IProductRepository Products { get; }
    }
}
