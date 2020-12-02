using KitchenRoutingSystem.Domain.DTOs;
using KitchenRoutingSystem.Shared.Commands;
using System.Collections.Generic;

namespace KitchenRoutingSystem.Sector.Salad.Commands.Request
{
    public class PrepareSaladRequest : CommandRequest
    {
        public string orderId { get; set; }
        public List<ProductDto> products { get; set; }
    }
}
