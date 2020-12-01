using KitchenRoutingSystem.Domain.Entities;
using KitchenRoutingSystem.Shared.Commands;
using System.Collections.Generic;

namespace KitchenRoutingSystem.Sector.Fries.Commands.Request
{
    public class PrepareFriesRequest : CommandRequest
    {
        public string orderId { get; set; }
        public List<Product> products { get; set; }
        
        public bool Invalid { get; set; }
        public bool Valid { get; set; }
    }
}
