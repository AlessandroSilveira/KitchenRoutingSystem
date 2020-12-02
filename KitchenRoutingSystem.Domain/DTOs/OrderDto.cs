using KitchenRoutingSystem.Shared.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace KitchenRoutingSystem.Domain.DTOs
{
    public class OrderDto : CommandRequest
    {
        public List<ProductDto> Products { get; set; }
    }
}
