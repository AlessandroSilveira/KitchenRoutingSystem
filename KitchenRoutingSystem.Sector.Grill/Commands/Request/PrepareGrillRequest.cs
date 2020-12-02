using KitchenRoutingSystem.Domain.DTOs;
using KitchenRoutingSystem.Shared.Commands;
using System.Collections.Generic;

namespace KitchenRoutingSystem.Sector.Grill.Commands.Request
{
    public class PrepareGrillRequest : CommandRequest
    {
        public string orderId { get; set; }
        public List<ProductDto> products { get; set; }
    }
}
