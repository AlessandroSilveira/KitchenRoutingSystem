using System;
using System.Collections.Generic;
using System.Text;

namespace KitchenRoutingSystem.Domain.Repository.UnitOfWork
{
    public interface IUnitOfWork
    {
        IOrderRepository Orders { get; }
        IProductRepository Products { get; }
        IOrderProductsRepository OrderProducts { get; }
    }
}
