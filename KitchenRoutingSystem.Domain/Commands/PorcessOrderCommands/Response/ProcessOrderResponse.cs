using KitchenRoutingSystem.Domain.Enums;
using System;

namespace KitchenRoutingSystem.Domain.Commands.PorcessOrderCommands.Response
{
    public class ProcessOrderResponse
    {
        public ProcessOrderResponse(string number, DateTime createDate, DateTime lastUpdateDate, decimal total, string notes, EOrderStatus status, ulong deliveryTag)
        {
            Number = number;
            CreateDate = createDate;
            LastUpdateDate = lastUpdateDate;
            Total = total;
            Notes = notes;
            Status = status;
            DeliveryTag = deliveryTag;
        }

        public string Number { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime LastUpdateDate { get; set; }       
        public decimal Total { get; set; }
        public string Notes { get; set; }
        public EOrderStatus Status { get; set; }
        public ulong DeliveryTag { get; set; }       
    }
}
