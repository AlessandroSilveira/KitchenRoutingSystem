﻿using KitchenRoutingSystem.Domain.DTOs;
using KitchenRoutingSystem.Domain.Enums;
using Shop.Domain.Shared.Entities;
using System;
using System.Collections.Generic;

namespace KitchenRoutingSystem.Domain.Entities
{
    public class Order : Entity
    {
        public Order(List<Product> product)
        {   
            Number = Guid.NewGuid().ToString().ToUpper().Substring(0, 8);
            CreateDate = DateTime.Now;
            LastUpdateDate = DateTime.Now;           
            Status = EOrderStatus.InTransit;
            Product = product;
        }

        public string Number { get; private set; }
        public DateTime CreateDate { get; private set; }        
        public DateTime LastUpdateDate { get; private set; }       
        public decimal Total => CalculateTotal();
        public string Notes { get; private set; }
        public EOrderStatus Status { get; private set; }        
        public ulong DeliveryTag { get; set; }
        public List<Product> Product { get; }

        public decimal CalculateTotal()
        {
            var total = 0M;
            foreach (var item in Product)
                total += item.Value * item.Quantity;

            return total;
        }
        public void StartDelivery()
        {
            Status = EOrderStatus.InTransit;
            LastUpdateDate = DateTime.Now;
        }

        public void Complete()
        {
            Status = EOrderStatus.Completed;
            LastUpdateDate = DateTime.Now;
        }

        public void RemoveProduct(Product product)
        {
            Product.Remove(product);
            CalculateTotal();
        }
    }
}