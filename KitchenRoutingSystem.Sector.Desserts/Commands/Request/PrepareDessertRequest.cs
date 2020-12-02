using KitchenRoutingSystem.Domain.DTOs;
using KitchenRoutingSystem.Shared.Commands;
using System.Collections.Generic;

namespace KitchenRoutingSystem.Sector.Desserts.Commands.Request
{
    public class PrepareDessertRequest : CommandRequest
    {
        public string orderId { get; set; }
        public List<ProductDto> products { get; set; }
    }
}
