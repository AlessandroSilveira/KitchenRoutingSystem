using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using MediatR;
using AutoMapper;
using System;

namespace KitchenRoutingSystem.Shared
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            //Mediator
            var assemblyDomain = AppDomain.CurrentDomain.Load("KitchenRoutingSystem.Domain");
            var assemblySectorFries = AppDomain.CurrentDomain.Load("KitchenRoutingSystem.Sector.Fries");
            var assemblySectorGrill = AppDomain.CurrentDomain.Load("KitchenRoutingSystem.Sector.Grill");
            var assemblySectorSalad = AppDomain.CurrentDomain.Load("KitchenRoutingSystem.Sector.Salad");
            var assemblySectorDrinks = AppDomain.CurrentDomain.Load("KitchenRoutingSystem.Sector.Drinks");
            var assemblySectorDessert = AppDomain.CurrentDomain.Load("KitchenRoutingSystem.Sector.Desserts");
            services.AddMediatR(assemblyDomain, assemblySectorFries, assemblySectorGrill, assemblySectorSalad, assemblySectorDrinks, assemblySectorDessert);



            //AutoMapper
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            
            return services;
        }
    }
}
