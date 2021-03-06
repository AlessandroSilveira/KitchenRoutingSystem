﻿using KitchenRoutingSystem.Domain.Enums;
using KitchenRoutingSystem.Shared.Commands;
using System;

namespace KitchenRoutingSystem.Domain.Commands.OrderProductCommands.Request
{
    public class OrderProductRequest : CommandRequest
    {
        public OrderProductRequest(Guid orderId, Guid productId, decimal value, int quantity, EProductType eProductType)
        {
            OrderId = orderId;
            ProductId = productId;
            Value = value;
            Quantity = quantity;
            ProductType = eProductType;
        }

        public Guid OrderId { get; private set; }
        public Guid ProductId { get; private set; }
        public EProductType ProductType { get; private set; }
        public decimal Value { get; private set; }
        public int Quantity { get; private set; }
    }
}
