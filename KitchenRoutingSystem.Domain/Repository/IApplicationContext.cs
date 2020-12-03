using KitchenRoutingSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace KitchenRoutingSystem.Domain.Repository
{
    public interface IApplicationContext
    {
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }


    }
}
