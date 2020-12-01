using KitchenRoutingSystem.Domain.Entities;
using KitchenRoutingSystem.Shared.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace KitchenRoutingSystem.Sector.Grill.Commands.Request
{
    public class PrepareGrillRequest : CommandRequest
    {
        public string OrderId { get; set; }
        public Product Product { get; set; }
    }
}
