using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KitchenRoutingSystem.Domain.Entities;
using KitchenRoutingSystem.Domain.Handlers.ProcessOrderHandlers;

namespace KitchenRoutingSystem.Api.MappingProfiles
{
    public class KitchenRoutingSystemMappingProfile : Profile
    {
        CreateMap<ProcessOrderHandler, Order>();
            CreateMap<ProcessOrderHandler, P>();
    }
}
