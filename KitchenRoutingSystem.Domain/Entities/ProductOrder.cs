using Shop.Domain.Shared.Entities;

namespace KitchenRoutingSystem.Domain.Entities
{
    public class ProductOrder : Entity
    {
        public ProductOrder(int orderId, Order order, int productId, Product products, int quantity, int price)
        {
            OrderId = orderId;
            Order = order;
            ProductId = productId;
            Product = products;
            Quantity = quantity;
            Price = price;
        }

        public int OrderId { get; private set; }        
        public virtual Order Order { get; private set; }
        public int ProductId { get; private set; }        
        public Product Product { get; private set; }
        public int Quantity { get; private set; }
        public int Price { get; private set; }
    }
}
