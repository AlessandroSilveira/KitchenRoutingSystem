using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using AutoMapper;

namespace KitchenRoutingSystem.Shared.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services) =>
            services.AddMediatR(Assembly.GetExecutingAssembly());


        public static IServiceCollection AddMapper(this IServiceCollection services)
        {
            return services.AddAutoMapper(Assembly.GetExecutingAssembly());
        }

    }
}
