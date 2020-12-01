using KitchenRoutingSystem.Domain.Entities;
using KitchenRoutingSystem.Shared.Commands;

namespace KitchenRoutingSystem.Sector.Salad.Commands.Request
{
    public class PrepareSaladRequest : CommandRequest
    {
        public string OrderId { get; set; }
        public Product Product { get; set; }
    }
}
