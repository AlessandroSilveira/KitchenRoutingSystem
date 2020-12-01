using Flunt.Validations;
using KitchenRoutingSystem.Domain.Entities;
using KitchenRoutingSystem.Domain.Enums;
using KitchenRoutingSystem.Shared.Commands;
using System;
using System.Collections.Generic;

namespace KitchenRoutingSystem.Domain.Commands.OrderCommands.Request
{
    public class CreateOrderRequest : CommandRequest
    {
        public string Number { get;  set; }
        public DateTime CreateDate { get;  set; }
        public DateTime LastUpdateDate { get;  set; }
        public List<Product> Products { get; set; }
        public decimal Total { get; set; }
        public string Notes { get;  set; }
        public EOrderStatus Status { get;  set; }

        public override void Validate()
        {
            AddNotifications(
                new Contract()
                    .Requires()
                    .IsNull(Products, "Products", "Order must have a product")
            );
        }
    }
}
