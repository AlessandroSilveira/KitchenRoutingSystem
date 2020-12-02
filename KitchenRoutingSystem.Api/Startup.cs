using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using KitchenRoutingSystem.Domain.Services.Interfaces;
using KitchenRoutingSystem.Domain.Services;
using KitchenRoutingSystem.Infra.Repositories;
using KitchenRoutingSystem.Domain.Repositories;
using KitchenRoutingSystem.Domain.Entities;
using KitchenRoutingSystem.Domain.MQ.OrderConsumer;
using KitchenRoutingSystem.Domain.MQ.OrderConsumerQueue;
using KitchenRoutingSystem.Sector.Salad;
using KitchenRoutingSystem.Sector.Salad.Services.Interfaces;
using KitchenRoutingSystem.Sector.Grill;
using KitchenRoutingSystem.Sector.Drinks;
using KitchenRoutingSystem.Sector.Dessert;
using KitchenRoutingSystem.Sector.Grill.Services.Interfaces;
using KitchenRoutingSystem.Sector.Drinks.Services.Interfaces;
using KitchenRoutingSystem.Sector.Grill.Services;
using KitchenRoutingSystem.Sector.Desserts.Services.Interfaces;
using KitchenRoutingSystem.Sector.Fries.Services.Interfaces;
using KitchenRoutingSystem.Sector.Fries.Services;
using KitchenRoutingSystem.Sector.Fries;
using KitchenRoutingSystem.Sector.Desserts.Services;
using KitchenRoutingSystem.Sector.Drinks.Services;
using KitchenRoutingSystem.Sector.Salad.Services;
using KitchenRoutingSystem.Domain.DTOs;
using AutoMapper;
using KitchenRoutingSystem.Domain.Commands.OrderCommands.Request;

namespace KitchenRoutingSystem.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var configuration = GetConfiguration();
            services.AddSingleton(typeof(IConfiguration), configuration);

            services.AddOptions();            

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "KitchenRoutingSystem.Api", Version = "v1" });
            });

            var assemblyDomain = AppDomain.CurrentDomain.Load("KitchenRoutingSystem.Domain");
            var assemblySectorFries = AppDomain.CurrentDomain.Load("KitchenRoutingSystem.Sector.Fries");
            var assemblySectorGrill = AppDomain.CurrentDomain.Load("KitchenRoutingSystem.Sector.Grill");
            var assemblySectorSalad = AppDomain.CurrentDomain.Load("KitchenRoutingSystem.Sector.Salad");
            var assemblySectorDrinks = AppDomain.CurrentDomain.Load("KitchenRoutingSystem.Sector.Drinks");
            var assemblySectorDessert = AppDomain.CurrentDomain.Load("KitchenRoutingSystem.Sector.Desserts");            

            services.AddMediatR(assemblyDomain, assemblySectorFries, assemblySectorGrill, assemblySectorSalad, assemblySectorDrinks, assemblySectorDessert);

            services.AddSingleton<IRepository<Order>, OrderRepository>();
            services.AddSingleton<IRepository<Product>, ProductRepository>();

            services.AddTransient<IOrderPublishService, OrderPublishServices>();
            services.AddTransient<IProcessProductService, ProcessProductService>();
            services.AddTransient<IFriesConsumerQueueService, FriesConsumerQueueService>();
            services.AddTransient<IGrillConsumerQueueService, GrillConsumerQueueService>();
            services.AddTransient<ISaladConsumerQueueService, SaladConsumerQueueService>();
            services.AddTransient<IDrinksConsumerQueueService, DrinksConsumerQueueService>();
            services.AddTransient<IDessertConsumerQueueService, DessertConsumerQueueService>();
            services.AddTransient<IOrderConsumerQueue, OrderConsumerQueue>();            

            services.AddSingleton<OrderConsumer>();
            services.AddSingleton<ConsumerFriesQueue>();
            services.AddSingleton<ConsumerGrillQueue>();
            services.AddSingleton<ConsumerSaladQueue>();
            services.AddSingleton<ConsumerDrinksQueue>();
            services.AddSingleton<ConsumerDessertQueue>();

            var config = new AutoMapper.MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ProductDto, Product>().ReverseMap();
                cfg.CreateMap<Order, CreateOrderRequest>().ReverseMap();
            });
            IMapper mapper = config.CreateMapper();
            services.AddSingleton(mapper);

        }
        private static IConfiguration GetConfiguration()
        {
            var configBuilder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true).AddEnvironmentVariables();
            return configBuilder.Build();
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, OrderConsumer orderConsumer, ConsumerFriesQueue consumerFriesQueue,
            ConsumerGrillQueue consumerGrillQueue, ConsumerSaladQueue consumerSaladQueue, ConsumerDrinksQueue consumerDrinkQueue, ConsumerDessertQueue consumerDessertQueue)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "KitchenRoutingSystem.Api v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            orderConsumer.StartConsumer().Wait();
            consumerFriesQueue.StartConsumer().Wait();
            consumerGrillQueue.StartConsumer().Wait();
            consumerSaladQueue.StartConsumer().Wait();
            consumerDrinkQueue.StartConsumer().Wait();
            consumerDessertQueue.StartConsumer().Wait();
        }
    }
}
