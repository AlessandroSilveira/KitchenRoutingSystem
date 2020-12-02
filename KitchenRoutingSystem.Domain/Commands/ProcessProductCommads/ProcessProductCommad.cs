using KitchenRoutingSystem.Domain.DTOs;
using KitchenRoutingSystem.Shared.Commands;
using System.Collections.Generic;

namespace KitchenRoutingSystem.Domain.Commands.ProcessProductCommads
{
    public class ProcessProductCommad : CommandRequest
    {
        public string orderId;
        public List<ProductDto> products { get; set; }
    }
}
