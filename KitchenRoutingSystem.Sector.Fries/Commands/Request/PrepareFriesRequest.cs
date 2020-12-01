using KitchenRoutingSystem.Domain.Entities;
using KitchenRoutingSystem.Shared.Commands;
using System.Collections.Generic;

namespace KitchenRoutingSystem.Sector.Fries.Commands.Request
{
    public class PrepareFriesRequest : CommandRequest
    {
        public string OrderId { get; set; }
        public List<Product> Product { get; set; }
    }
}
