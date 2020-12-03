using KitchenRoutingSystem.Domain.Entities;
using KitchenRoutingSystem.Domain.Repository;
using Microsoft.EntityFrameworkCore;

namespace KitchenRoutingSystem.Infra.Context
{
    public class ApplicationContext : DbContext, IApplicationContext
    {
        public ApplicationContext()
        {
        }

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
        }

        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
       

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Order>().ToTable("Order");
            modelBuilder.Entity<Product>().ToTable("Product");
            
        }
    }
}
