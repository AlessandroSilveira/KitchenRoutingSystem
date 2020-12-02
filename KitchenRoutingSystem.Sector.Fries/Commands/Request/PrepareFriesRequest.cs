using KitchenRoutingSystem.Domain.DTOs;
using KitchenRoutingSystem.Shared.Commands;
using System.Collections.Generic;

namespace KitchenRoutingSystem.Sector.Fries.Commands.Request
{
    public class PrepareFriesRequest : CommandRequest
    {
        public string orderId { get; set; }
        public List<ProductDto> products { get; set; }
    }

}

