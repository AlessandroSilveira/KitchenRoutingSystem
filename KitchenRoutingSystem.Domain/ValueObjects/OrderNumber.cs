using KitchenRoutingSystem.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace KitchenRoutingSystem.Domain.ValueObjects
{
    public class OrderNumber : ValueObject
    {
        public OrderNumber()
        {
            Number = Guid.NewGuid().ToString().ToUpper().Substring(0, 8);
        }

        public string Number { get; private set; }
    }
}
