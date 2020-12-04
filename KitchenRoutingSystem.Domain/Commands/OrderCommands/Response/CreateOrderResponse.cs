using KitchenRoutingSystem.Domain.DTOs;
using KitchenRoutingSystem.Domain.Entities;
using KitchenRoutingSystem.Domain.Enums;
using System;
using System.Collections.Generic;

namespace KitchenRoutingSystem.Domain.Commands.OrderCommands.Response
{
    public class CreateOrderResponse 
    {
        public CreateOrderResponse(string number, DateTime createDate, DateTime lastUpdateDate, List<ProductDto> products, decimal total, string notes, EOrderStatus status)
        {
            Number = number;
            CreateDate = createDate;
            LastUpdateDate = lastUpdateDate;
            Products = products;
            Total = total;
            Notes = notes;
            Status = status;
        }

        public string Number { get; private set; }
        public DateTime CreateDate { get; private set; }
        public DateTime LastUpdateDate { get; private set; }
        public List<ProductDto> Products { get; set; }
        public decimal Total { get; set; }
        public string Notes { get; private set; }
        public EOrderStatus Status { get; private set; }        

        
    }
}
