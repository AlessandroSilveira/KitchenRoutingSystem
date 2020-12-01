using System;
using System.Collections.Generic;
using KitchenRoutingSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace KitchenRoutingSystem.Infra.Context
{
    public class OrderContext : DbContext
    {
        public OrderContext()
        {
        }

        public OrderContext(DbContextOptions<OrderContext> options)
            : base(options)
        {
            var orders = new[]
            {
               new Order(new List<Product>
                   {
                       new Product
                       {
                           ProductId = "1",
                           Quantity = 1,
                           Value = 1,
                           ProductType = Domain.Enums.EProductType.Fries
                       }
                    },
                    "Order Created")
            };

            Order.AddRange(orders);
            SaveChanges();

            var products = new[]
        {
            new Product{ProductId = "1", Quantity = 10, Value = 10,  ProductType = Domain.Enums.EProductType.Fries },
            new Product { ProductId = "2", Quantity = 1, Value = 1, ProductType = Domain.Enums.EProductType.Grill },
            new Product { ProductId = "3", Quantity = 1, Value = 1, ProductType = Domain.Enums.EProductType.Salad },
            new Product{ProductId = "4", Quantity = 1, Value = 1,  ProductType = Domain.Enums.EProductType.Drink },
            new Product{ProductId = "5", Quantity = 1, Value = 1,  ProductType = Domain.Enums.EProductType.Dessert }
        };

            Products.AddRange(products);
            SaveChanges();
        }

        public virtual DbSet<Order> Order { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>(entity =>
            {
                entity.Property(e => e.Number).IsRequired();
                entity.Property(e => e.CreateDate).IsRequired();
                entity.Property(e => e.LastUpdateDate).IsRequired();
                entity.Property(e => e.Products).IsRequired();
                entity.Property(e => e.Notes).IsRequired();
                entity.Property(e => e.Status).IsRequired();
                entity.Property(e => e.DeliveryTag).IsRequired();
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(e => e.ProductId).IsRequired();
                entity.Property(e => e.Value).IsRequired();
                entity.Property(e => e.Quantity).IsRequired();
                entity.Property(e => e.ProductType).IsRequired();
            });
        }
    }
}