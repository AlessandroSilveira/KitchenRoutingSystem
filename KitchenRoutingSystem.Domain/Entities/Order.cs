using KitchenRoutingSystem.Domain.DTOs;
using KitchenRoutingSystem.Domain.Enums;
using Shop.Domain.Shared.Entities;
using System;
using System.Collections.Generic;

namespace KitchenRoutingSystem.Domain.Entities
{
    public class Order : Entity
    {
        public Order()
        {   
            Number = Guid.NewGuid().ToString().ToUpper().Substring(0, 8);
            CreateDate = DateTime.Now;
            LastUpdateDate = DateTime.Now;           
            Status = EOrderStatus.InTransit;            
        }

        public string Number { get; private set; }
        public DateTime CreateDate { get; private set; }        
        public DateTime LastUpdateDate { get; private set; }       
        public decimal Total { get; set; }
        public string Notes { get; private set; }
        public EOrderStatus Status { get; private set; }        
        public ulong DeliveryTag { get; set; }        

    
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
                
    }
}