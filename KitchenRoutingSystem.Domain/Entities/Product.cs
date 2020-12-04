using KitchenRoutingSystem.Domain.Enums;

namespace KitchenRoutingSystem.Domain.Entities
{
    public class Product
    {  
        public string ProductId { get; set; }
        public decimal Value { get;  set; }
        public int Quantity { get;  set; }
        public EProductType ProductType {get;  set;}
        public EProductStatus Status { get; set; }

    }

   
}
