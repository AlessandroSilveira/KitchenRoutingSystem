using KitchenRoutingSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace KitchenRoutingSystem.Domain.DTOs
{
    public class ProductDto
    {
        public string ProductId { get; set; }
        public decimal Value { get; set; }
        public int Quantity { get; set; }
        public EProductType ProductType { get; set; }
        public EProductStatus Status { get; set; }
    }
}
