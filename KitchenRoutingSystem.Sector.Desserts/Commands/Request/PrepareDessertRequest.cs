using KitchenRoutingSystem.Domain.Entities;
using KitchenRoutingSystem.Shared.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace KitchenRoutingSystem.Sector.Desserts.Commands.Request
{
    public class PrepareDessertRequest : CommandRequest
    {
        public string OrderId { get; set; }
        public Product Product { get; set; }
    }
}
