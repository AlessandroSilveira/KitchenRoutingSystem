using KitchenRoutingSystem.Domain.Entities;
using KitchenRoutingSystem.Shared.Commands;


namespace KitchenRoutingSystem.Sector.Drinks.Commands.Request
{
    public class PrepareDrinksRequest : CommandRequest
    {
        public string OrderId { get; set; }
        public Product Product { get; set; }
    }
}
