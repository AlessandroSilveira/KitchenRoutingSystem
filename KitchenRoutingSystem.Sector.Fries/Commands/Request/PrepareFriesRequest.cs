using KitchenRoutingSystem.Domain.Entities;
using KitchenRoutingSystem.Shared.Commands;

namespace KitchenRoutingSystem.Sector.Fries.Commands.Request
{
    public class PrepareFriesRequest :  CommandRequest
    {
        public string OrderId { get; set; }
        public Product Product { get; set; }
    }
}
