using KitchenRoutingSystem.Domain.DTOs;
using KitchenRoutingSystem.Domain.Entities;
using KitchenRoutingSystem.Shared.Commands;
using System.Collections.Generic;

namespace KitchenRoutingSystem.Sector.Drinks.Commands.Request
{
    public class PrepareDrinksRequest : CommandRequest
    {
        public string orderId { get; set; }
        public List<ProductDto> products { get; set; }
    }
}
