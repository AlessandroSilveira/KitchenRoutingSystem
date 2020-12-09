using KitchenRoutingSystem.Domain.Enums;
using Shop.Domain.Shared.Entities;
using System;

namespace KitchenRoutingSystem.Domain.Entities
{
    public class OrderProduct : Entity
    {
        public OrderProduct(Guid orderId, Guid productId, decimal value, int quantity, EProductType productType)
        {
            OrderId = orderId;
            ProductId = productId;
            Value = value;
            Quantity = quantity;
            ProductType = productType;
        }

        public Guid OrderId { get; private set; }
        public Guid ProductId { get; private set; }
        public decimal Value { get; private set; }
        public int Quantity { get; private set; }
        public EProductType ProductType { get; private set; }

        public void ChangeQuantity(int quantity)
        {
            Quantity = quantity;
        }
    }
}
