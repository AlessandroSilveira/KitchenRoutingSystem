using AutoMapper;
using KitchenRoutingSystem.Domain.Entities;
using KitchenRoutingSystem.Domain.Handlers.ProcessOrderHandlers;
using KitchenRoutingSystem.Sector.Fries.Handlers.PrepareFriesHandler;

namespace KitchenRoutingSystem.Api.MappingProfiles
{
    public class KitchenRoutingSystemMappingProfile : Profile
    {
        public KitchenRoutingSystemMappingProfile()
        {
            CreateMap<ProcessOrderHandler, Order>().ReverseMap();
            CreateMap<ProcessOrderHandler, Product>().ReverseMap();

            CreateMap<PrepareFriesHandler, Order>().ReverseMap();
            CreateMap<PrepareFriesHandler, Product>().ReverseMap();

        }

    }
}
