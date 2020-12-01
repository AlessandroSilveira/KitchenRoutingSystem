using KitchenRoutingSystem.Infra.Context;
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
using KitchenRoutingSystem.Sector.Fries;
using KitchenRoutingSystem.Sector.Fries.Services.Interfaces;

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
            services.AddDbContext<OrderContext>(opt => opt.UseInMemoryDatabase("KitchenRoutingServiceBase"));

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "KitchenRoutingSystem.Api", Version = "v1" });
            });

            var assemblyDomain = AppDomain.CurrentDomain.Load("KitchenRoutingSystem.Domain");
            var assemblySectorFries = AppDomain.CurrentDomain.Load("KitchenRoutingSystem.Sector.Fries");
            services.AddMediatR(assemblyDomain, assemblySectorFries);

            services.AddSingleton<IRepository<Order>, OrderRepository>();
            services.AddSingleton<IRepository<Product>, ProductRepository>();

            services.AddTransient<IOrderPublishService, OrderPublishServices>();
            services.AddTransient<IProcessProductService, ProcessProductService>();
            services.AddTransient<IFriesConsumerQueueService, FriesConsumerQueueService>();
            

            services.AddTransient<IOrderConsumerQueue, OrderConsumerQueue>();
            services.AddSingleton<OrderConsumer>();
            services.AddSingleton<ConsumerFriesQueue>();
        }
        private static IConfiguration GetConfiguration()
        {
            var configBuilder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true).AddEnvironmentVariables();
            return configBuilder.Build();
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, OrderConsumer orderConsumer, ConsumerFriesQueue consumerFriesQueue)
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
        }
    }
}
