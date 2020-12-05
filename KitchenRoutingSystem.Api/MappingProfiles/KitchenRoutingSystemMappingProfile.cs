using AutoMapper;
using KitchenRoutingSystem.Domain.Entities;
using KitchenRoutingSystem.Domain.Handlers.CreateOrderHandlers;
using KitchenRoutingSystem.Sector.Fries.Handlers.PrepareFriesHandler;

namespace KitchenRoutingSystem.Api.MappingProfiles
{
    public class KitchenRoutingSystemMappingProfile : Profile
    {
        public KitchenRoutingSystemMappingProfile()
        {
            CreateMap<CreateOrderHandler, Order>().ReverseMap();
            CreateMap<CreateOrderHandler, Product>().ReverseMap();

            CreateMap<PrepareFriesHandler, Order>().ReverseMap();
            CreateMap<PrepareFriesHandler, Product>().ReverseMap();

        }

    }
}
