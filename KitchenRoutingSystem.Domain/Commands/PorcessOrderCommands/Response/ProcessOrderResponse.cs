using KitchenRoutingSystem.Domain.DTOs;
using KitchenRoutingSystem.Domain.Entities;
using KitchenRoutingSystem.Domain.Enums;
using System;
using System.Collections.Generic;

namespace KitchenRoutingSystem.Domain.Commands.PorcessOrderCommands.Response
{
    public class ProcessOrderResponse
    {
        public ProcessOrderResponse(string number, DateTime createDate, DateTime lastUpdateDate, List<ProductDto> products, decimal total, string notes, EOrderStatus status, ulong deliveryTag)
        {
            Number = number;
            CreateDate = createDate;
            LastUpdateDate = lastUpdateDate;
            Products = products;
            Total = total;
            Notes = notes;
            Status = status;
            DeliveryTag = deliveryTag;
        }

        public string Number { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime LastUpdateDate { get; set; }
        public List<ProductDto> Products { get; set; }
        public decimal Total { get; set; }
        public string Notes { get; set; }
        public EOrderStatus Status { get; set; }

        public ulong DeliveryTag { get; set; }

       
    }
}
